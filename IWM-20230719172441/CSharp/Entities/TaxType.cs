using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class TaxType : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class TaxTypeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DecimalFilter Percentage { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<TaxTypeFilter> OrFilter { get; set; }
        public TaxTypeOrder OrderBy {get; set;}
        public TaxTypeSelect Selects {get; set;}
        public TaxTypeSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxTypeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Percentage = 3,
        Status = 4,
        Used = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum TaxTypeSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Percentage = E._3,
        Status = E._4,
        Used = E._8,
    }

    [Flags]
    public enum TaxTypeSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
    }
}
