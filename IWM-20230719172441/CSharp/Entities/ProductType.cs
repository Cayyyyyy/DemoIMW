using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class ProductType : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ProductTypeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ProductTypeFilter> OrFilter { get; set; }
        public ProductTypeOrder OrderBy {get; set;}
        public ProductTypeSelect Selects {get; set;}
        public ProductTypeSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductTypeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        Status = 4,
        Used = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ProductTypeSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        Status = E._4,
        Used = E._8,
    }

    [Flags]
    public enum ProductTypeSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Description = E._3,
    }
}
