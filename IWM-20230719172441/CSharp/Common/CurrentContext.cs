using System;
using System.Collections.Generic;
using TrueSight.Common;
namespace IWM.Common
{
    public interface ICurrentContext : IServiceScoped
    {
        long GlobalUserId { get; set; }
        long GlobalUserTypeId { get; set; }
        long UserId { get; set; }
        string UserName { get; set; }
        Guid UserRowId { get; set; }
        int TimeZone { get; set; }
        string Language { get; set; }
        string Token { get; set; }
        List<long> RoleIds { get; set; }
        Dictionary<long, List<FilterPermissionDefinition>> Filters { get; set; }
    }

    public class CurrentContext : ICurrentContext
    {
        public long GlobalUserId { get; set; }
        public long GlobalUserTypeId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public Guid UserRowId { get; set; }
        public int TimeZone { get; set; }
        public string Language { get; set; } = "vi";
        public string Token { get; set; }
        public List<long> RoleIds { get; set; }
        public Dictionary<long, List<FilterPermissionDefinition>> Filters { get; set; }
    }
}
