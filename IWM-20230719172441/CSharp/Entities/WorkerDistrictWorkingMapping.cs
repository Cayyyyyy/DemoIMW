using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class WorkerDistrictWorkingMapping : DataEntity
    {
        public long WorkerId { get; set; }
        public long DistrictId { get; set; }
        public District District { get; set; }
        public Worker Worker { get; set; }
    }

    public class WorkerDistrictWorkingMappingFilter : FilterEntity
    {
        public IdFilter WorkerId { get; set; }
        public IdFilter DistrictId { get; set; }
        public List<WorkerDistrictWorkingMappingFilter> OrFilter { get; set; }
        public WorkerDistrictWorkingMappingOrder OrderBy {get; set;}
        public WorkerDistrictWorkingMappingSelect Selects {get; set;}
        public WorkerDistrictWorkingMappingSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkerDistrictWorkingMappingOrder
    {
        Worker = 0,
        District = 1,
    }

    [Flags]
    public enum WorkerDistrictWorkingMappingSelect:long
    {
        ALL = E.ALL,
        Worker = E._0,
        District = E._1,
    }

    [Flags]
    public enum WorkerDistrictWorkingMappingSearch:long
    {
        ALL = E.ALL,
    }
}
