using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class Ward : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long DistrictId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public District District { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class WardFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<WardFilter> OrFilter { get; set; }
        public WardOrder OrderBy {get; set;}
        public WardSelect Selects {get; set;}
        public WardSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WardOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Priority = 3,
        District = 4,
        Status = 5,
        Used = 10,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum WardSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Priority = E._3,
        District = E._4,
        Status = E._5,
        Used = E._10,
    }

    [Flags]
    public enum WardSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
    }
}
