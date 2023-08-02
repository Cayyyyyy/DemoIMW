using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using IWM.Common;
using IWM.Handlers;
using IWM.Models;
using IWM.Rpc;
using IWM.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thinktecture;
using TrueSight.Caching;
using TrueSight.Common;
using TrueSight.DynamicTemplate;
using TrueSight.PER;
using TrueSight.RQHistory;
using Z.EntityFramework.Extensions;
using TrueSight.RabbitMQ.Configuration;

namespace IWM
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
            License.Activate(Configuration["TrueSightLicense"]);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = DataEntity.InformationResource;
            _ = DataEntity.WarningResource;
            _ = DataEntity.ErrorResource;

            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK";
                });

            StaticParameter.ModuleName = StaticParams.ModuleName;
            StaticParameter.SiteCode = StaticParams.SiteCode;

            services.AddSingleton<IRedisStore>(
                new RedisStore(hostName: Configuration["Redis:Hostname"],
                port: int.Parse(Configuration["Redis:Port"]),
                instance: int.Parse(Configuration["Redis:Instance"])));

            Assembly[] assemblies = new[] { 
                Assembly.GetAssembly(typeof(IServiceScoped)),
                Assembly.GetAssembly(typeof(Startup)) 
            };

            services.AddRabbitMQConfiguration(new RabbitMQBuilder
            {
                Hostname = Configuration["RabbitConfig:Hostname"],
                Username = Configuration["RabbitConfig:Username"],
                Password = Configuration["RabbitConfig:Password"],
                VirtualHost = Configuration["RabbitConfig:VirtualHost"],
                Port = int.Parse(Configuration["RabbitConfig:Port"]),
            }, assemblies);

            PermissionBuilder.ConnectionString = Configuration.GetConnectionString("DataContext");
            services.AddPermission(Configuration.GetConnectionString("DataContext"), new RedisConfig
            {
                Hostname = Configuration["Redis:Hostname"],
                Port = int.Parse(Configuration["Redis:Port"]),
                Instance = int.Parse(Configuration["Redis:Instance"]),
                Prefix = StaticParams.ModuleName,
                Domain = ""
            });

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
                {
                    sqlOptions.AddBulkOperationSupport();
                });
                options.AddInterceptors(new HintCommandInterceptor());
            });
            EntityFrameworkManager.ContextFactory = context =>
            {
               var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
                {
                    sqlOptions.AddBulkOperationSupport();
                });
                DataContext DataContext = new DataContext(optionsBuilder.Options);
                return DataContext;
            };

            services.AddRequestHistory(url: Configuration["ElasticConfig:HostName"],
                username: Configuration["ElasticConfig:Username"],
                password: Configuration["ElasticConfig:Password"],
                prefix: Configuration["ElasticConfig:Prefix"],
                Configuration["InternalServices:UTILS"]);

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                     .AsImplementedInterfaces()
                     .WithScopedLifetime());
            
            services.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(Configuration.GetConnectionString("DataContext"), new SqlServerStorageOptions
             {
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(2),
                 QueuePollInterval = TimeSpan.FromSeconds(10),
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                 UseRecommendedIsolationLevel = true,
                 UsePageLocksOnDequeue = true,
                 DisableGlobalLocks = true
             }));
            services.AddHangfireServer();

            services.AddAuthenticationAndSwagger(Configuration["Config:PublicRSAKey"]);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy =>
                    policy.Requirements.Add(new PermissionRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, SimpleHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Simple", policy =>
                    policy.Requirements.Add(new SimpleRequirement()));
            });

            Action onChange = () =>
            {
                JobStorage.Current = new SqlServerStorage(Configuration.GetConnectionString("DataContext"));
                using (var connection = JobStorage.Current.GetConnection())
                {
                    foreach (var recurringJob in connection.GetRecurringJobs())
                    {
                        RecurringJob.RemoveIfExists(recurringJob.Id);
                    }
                }

                RecurringJob.AddOrUpdate<MaintenanceService>("CleanHangfire", x => x.CleanHangfire(), Cron.Daily);
            };
            onChange();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), onChange);
            services.AddDynamicTemplate(InternalServices.PORTAL, InternalServices.UTILS);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<BodyMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "rpc/iwm/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/rpc/iwm/swagger/v1/swagger.json", "iwm API");
                c.RoutePrefix = "rpc/iwm/swagger";
            });
            app.UseDeveloperExceptionPage();
            app.UseHangfireDashboard("/rpc/iwm/hangfire");
            
        }
    }
}
