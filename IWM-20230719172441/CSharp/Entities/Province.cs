using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class Province : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ProvinceFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ProvinceFilter> OrFilter { get; set; }
        public ProvinceOrder OrderBy {get; set;}
        public ProvinceSelect Selects {get; set;}
        public ProvinceSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProvinceOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Priority = 3,
        Status = 4,
        Used = 9,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ProvinceSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Priority = E._3,
        Status = E._4,
        Used = E._9,
    }

    [Flags]
    public enum ProvinceSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
    }
}
