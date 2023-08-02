using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class ProvinceGrouping : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public long? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public long Level { get; set; }
        public string Path { get; set; }
        public ProvinceGrouping Parent { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ProvinceGroupingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ParentId { get; set; }
        public bool? HasChildren { get; set; }
        public LongFilter Level { get; set; }
        public StringFilter Path { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ProvinceGroupingFilter> OrFilter { get; set; }
        public ProvinceGroupingOrder OrderBy { get; set; }
        public ProvinceGroupingSelect Selects { get; set; } = ProvinceGroupingSelect.ALL;
        public ProvinceGroupingSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProvinceGroupingOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
        Parent = 4,
        HasChildren = 5,
        Level = 6,
        Path = 7,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ProvinceGroupingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
        Parent = E._4,
        HasChildren = E._5,
        Level = E._6,
        Path = E._7,
    }

    [Flags]
    public enum ProvinceGroupingSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Path = E._7,
    }
}
