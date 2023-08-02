using IWM.Common;
using IWM.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Common;

namespace IWM.Rpc
{
    public class Root
    {
        protected const string Module = "iwm";
        protected const string Rpc = "rpc/";
        protected const string Rest = "rest/";
    }
    [Authorize]
    [Authorize(Policy ="Permission")]
    public class RpcController : ControllerBase
    {
    }

    [Authorize]
    [Authorize(Policy = "Simple")]
    public class SimpleController : ControllerBase
    {
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement()
        {
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private ICurrentContext CurrentContext;
        private IConfiguration Configuration;

        private readonly IHttpContextAccessor httpContextAccessor;
        private IUOW UOW;
        public PermissionHandler(
            IConfiguration Configuration,
            ICurrentContext CurrentContext,
            IUOW UOW,
            IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
            this.Configuration = Configuration;
            this.UOW = UOW;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var types = context.User.Claims.Select(c => c.Type).ToList();
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return;
            }
            
            var HttpContext = httpContextAccessor.HttpContext;

            if (Configuration["OnCloud"].ParseBool())
            {
                CurrentContext.LoadConfigurationByDomain(HttpContext.Request.Host.Value, Configuration["MasterCloudDomain"]);
            }
            else
            {
                CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            }

            CurrentContext.LoadContextData(context.User, httpContextAccessor.HttpContext.Request);
            UOW.LoadConfiguration();

            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            CurrentContext.RoleIds = await CurrentContext.GetRoles(url);
            CurrentContext.Filters = await CurrentContext.GetPermissionFilter(url);
            if (CurrentContext.Filters.Count == 0)
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }

    public class SimpleRequirement : IAuthorizationRequirement
    {
        public SimpleRequirement()
        {
        }
    }
    public class SimpleHandler : AuthorizationHandler<SimpleRequirement>
    {
        private IConfiguration Configuration;
        private ICurrentContext CurrentContext;
        private IUOW UOW;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SimpleHandler(ICurrentContext CurrentContext, IUOW UOW, IHttpContextAccessor httpContextAccessor, IConfiguration Configuration)
        {
            this.CurrentContext = CurrentContext;
            this.Configuration = Configuration;
            this.UOW = UOW;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SimpleRequirement requirement)
        {
            var types = context.User.Claims.Select(c => c.Type).ToList();
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return;
            }
            
            var HttpContext = httpContextAccessor.HttpContext;

            if (Configuration["OnCloud"].ParseBool())
            {
                CurrentContext.LoadConfigurationByDomain(HttpContext.Request.Host.Value, Configuration["MasterCloudDomain"]);
            }
            else
            {
                CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            }

            CurrentContext.LoadContextData(context.User, httpContextAccessor.HttpContext.Request);
            UOW.LoadConfiguration();

            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            context.Succeed(requirement);
        }
    }
}