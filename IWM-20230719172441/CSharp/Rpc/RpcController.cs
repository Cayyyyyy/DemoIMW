using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrueSight.PER;

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
        private readonly IUOW UOW;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PermissionHandler(
            ICurrentContext CurrentContext, 
            IUOW UOW, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
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
            long UserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
            long GlobalUserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.Sid).Value, out long gu) ? gu : 0;
            long GlobalUserTypeId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.GroupSid).Value, out long gru) ? gru : 0;
            string UserName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            var HttpContext = httpContextAccessor.HttpContext;
            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            string TimeZone = HttpContext.Request.Headers["X-TimeZone"];
            string Language = HttpContext.Request.Headers["X-Language"];
            CurrentContext.Token = HttpContext.Request.Cookies["Token"] != null ?
                HttpContext.Request.Cookies["Token"] :
                HttpContext.GetTokenAsync("access_token").Result;
            HttpContext.Request.Headers["AppUserId"] = UserId.ToString();
            HttpContext.Request.Headers["AppUser"] = UserName;
            CurrentContext.UserId = UserId;
            CurrentContext.GlobalUserId = GlobalUserId;
            CurrentContext.GlobalUserTypeId = GlobalUserTypeId;
            CurrentContext.UserName = UserName;
            CurrentContext.TimeZone = int.TryParse(TimeZone, out int t) ? t : 0;
            CurrentContext.Language = Language ?? "vi";       
            CurrentContext.RoleIds = await PermissionBuilder.GetRoles(UserId, url);
            CurrentContext.Filters = await PermissionBuilder.GetPermissionFilter(UserId, url);
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
        private ICurrentContext CurrentContext;
        private readonly IUOW UOW;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SimpleHandler(ICurrentContext CurrentContext, IUOW UOW, IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
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
            long UserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
            long GlobalUserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.Sid).Value, out long gu) ? gu : 0;
            long GlobalUserTypeId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.GroupSid).Value, out long gru) ? gru : 0;
            Guid UserRowId = Guid.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.PrimarySid).Value, out Guid rowId) ? rowId : Guid.Empty;
            string UserName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            var HttpContext = httpContextAccessor.HttpContext;
            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            string TimeZone = HttpContext.Request.Headers["X-TimeZone"];
            string Language = HttpContext.Request.Headers["X-Language"];
            string Latitude = HttpContext.Request.Headers["X-Latitude"];
            string Longitude = HttpContext.Request.Headers["X-Longitude"];
            HttpContext.Request.Headers["AppUserId"] = UserId.ToString();
            HttpContext.Request.Headers["AppUser"] = UserName;
            CurrentContext.Token = HttpContext.Request.Cookies["Token"] != null ?
                HttpContext.Request.Cookies["Token"] :
                HttpContext.GetTokenAsync("access_token").Result;
            CurrentContext.UserId = UserId;
            CurrentContext.GlobalUserId = GlobalUserId;
            CurrentContext.GlobalUserTypeId = GlobalUserTypeId;
            CurrentContext.UserRowId = UserRowId;
            CurrentContext.TimeZone = int.TryParse(TimeZone, out int t) ? t : 0;
            CurrentContext.Language = Language ?? "vi";

            context.Succeed(requirement);
        }
    }
}