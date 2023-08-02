using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class Status : EnumEntity
    {
        public static Status INACTIVE = new Status (Id : 0, Code : "INACTIVE", Name : "Ngừng hoạt động", Color : "#525252");
        public static Status ACTIVE = new Status (Id : 1, Code : "ACTIVE", Name : "Hoạt động", Color: "#24a148");
        public static List<Status> StatusEnumList = new List<Status>
        {
            INACTIVE, ACTIVE,
        };
        public Status() : base(nameof(Status)) { }
        public Status(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(Status), Color, Value) { }
    }

    public class StatusFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Color { get; set; }
        public List<StatusFilter> OrFilter { get; set; }
        public StatusOrder OrderBy { get; set; }
        public StatusSelect Selects { get; set; } = StatusSelect.ALL;
        public StatusSearch SearchBy { get; set; }
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
    public enum StatusSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Color = E._3,
    }

    [Flags]
    public enum StatusSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Color = E._3,
    }
}
