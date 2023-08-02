using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class UnitOfMeasure : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public bool IsDecimal { get; set; }
        public bool Used { get; set; }
        public string ErpCode { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class UnitOfMeasureFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? IsDecimal { get; set; }
        public bool? Used { get; set; }
        public StringFilter ErpCode { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<UnitOfMeasureFilter> OrFilter { get; set; }
        public UnitOfMeasureOrder OrderBy {get; set;}
        public UnitOfMeasureSelect Selects {get; set;}
        public UnitOfMeasureSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UnitOfMeasureOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        Status = 4,
        IsDecimal = 5,
        Used = 9,
        ErpCode = 11,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum UnitOfMeasureSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        Status = E._4,
        IsDecimal = E._5,
        Used = E._9,
        ErpCode = E._11,
    }

    [Flags]
    public enum UnitOfMeasureSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        ErpCode = E._11,
    }
}
