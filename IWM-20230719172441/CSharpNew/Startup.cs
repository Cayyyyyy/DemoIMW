using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using IWM.Common;
using IWM.Rpc;
using System;
using System.Reflection;
using TrueSight;
using TrueSight.Common;

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
            Assembly[] assemblies = new[] {
                Assembly.GetAssembly(typeof(ICapSubscribe)),
                Assembly.GetAssembly(typeof(IServiceScoped)),
                Assembly.GetAssembly(typeof(Startup))
            };
            TrueSightBuilder.ModuleName = StaticParams.ModuleName;
            TrueSightBuilder.SiteCode = StaticParams.SiteCode;
            services.AddTrueSight(assemblies, Configuration["RabbitConnectionString"], Configuration["MongoConnectionString"], Configuration["RedisConnectionString"], Configuration["Config:PublicRSAKey"]);

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
                InternalServices.APPROVAL_FLOW = Configuration["InternalServices:APPROVAL_FLOW"];            
                InternalServices.ES = Configuration["InternalServices:ES"];            
                InternalServices.PORTAL = Configuration["InternalServices:PORTAL"];
                InternalServices.UTILS_REQUEST_HISTORY = Configuration["InternalServices:UTILS_REQUEST_HISTORY"];
                InternalServices.UTILS_STORAGE = Configuration["InternalServices:UTILS_STORAGE"];
            };
            onChange();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), onChange);
            StaticParams.ConnectionManager = ConnectionManager.CreateInstance(
                tenantId: 1,
                domain: "",
                Configuration,
                new InternalService(
                    Portal: InternalServices.PORTAL,
                    EventSourcing: null,
                    UtilsStorage: InternalServices.UTILS_STORAGE,
                    UtilsRequestHistory: InternalServices.UTILS_REQUEST_HISTORY,
                    ApprovalFlow: InternalServices.APPROVAL_FLOW),
                new ExtensionServiceEnableConfig()
            );
        }

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
        }
    }
}