using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class WorkerGroup : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
    }

    public class WorkerGroupFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public List<WorkerGroupFilter> OrFilter { get; set; }
        public WorkerGroupOrder OrderBy { get; set; }
        public WorkerGroupSelect Selects { get; set; } = WorkerGroupSelect.ALL;
        public WorkerGroupSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkerGroupOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
    }

    [Flags]
    public enum WorkerGroupSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
    }

    [Flags]
    public enum WorkerGroupSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
    }
}
