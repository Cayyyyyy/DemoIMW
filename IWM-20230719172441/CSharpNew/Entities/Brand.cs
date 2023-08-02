using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class Brand : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public string Description { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class BrandFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter Description { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<BrandFilter> OrFilter { get; set; }
        public BrandOrder OrderBy { get; set; }
        public BrandSelect Selects { get; set; } = BrandSelect.ALL;
        public BrandSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
        Description = 4,
        Used = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum BrandSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
        Description = E._4,
        Used = E._8,
    }

    [Flags]
    public enum BrandSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Description = E._4,
    }
}
