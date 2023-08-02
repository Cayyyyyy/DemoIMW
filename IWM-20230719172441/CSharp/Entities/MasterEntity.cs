using TrueSight.Common;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Common
{
    public class MasterEntity : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public long IsTree { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class MasterEntityFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public LongFilter IsTree { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<MasterEntityFilter> OrFilter { get; set; }
        public MasterEntityOrder OrderBy { get; set; }
        public MasterEntitySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MasterEntityOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 6,
        IsTree = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum MasterEntitySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._6,
        IsTree = E._8,
    }
}
