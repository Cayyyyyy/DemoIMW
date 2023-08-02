using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class Status : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class StatusFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Color { get; set; }
        public List<StatusFilter> OrFilter { get; set; }
        public StatusOrder OrderBy {get; set;}
        public StatusSelect Selects {get; set;}
        public StatusSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Color = 3,
    }

    [Flags]
    public enum StatusSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Color = E._3,
    }

    [Flags]
    public enum StatusSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Color = E._3,
    }
}
