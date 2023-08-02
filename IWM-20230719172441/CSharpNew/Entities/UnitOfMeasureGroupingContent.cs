using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class UnitOfMeasureGroupingContent : DataEntity
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal? Factor { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public UnitOfMeasureGrouping UnitOfMeasureGrouping { get; set; }
        public Guid RowId { get; set; }
    }

    public class UnitOfMeasureGroupingContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Factor { get; set; }
        public List<UnitOfMeasureGroupingContentFilter> OrFilter { get; set; }
        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
        public UnitOfMeasureGroupingContentSelect Selects { get; set; } = UnitOfMeasureGroupingContentSelect.ALL;
        public UnitOfMeasureGroupingContentSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UnitOfMeasureGroupingContentOrder
    {
        Id = 0,
        UnitOfMeasureGrouping = 1,
        UnitOfMeasure = 2,
        Factor = 3,
    }

    [Flags]
    public enum UnitOfMeasureGroupingContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        UnitOfMeasureGrouping = E._1,
        UnitOfMeasure = E._2,
        Factor = E._3,
    }

    [Flags]
    public enum UnitOfMeasureGroupingContentSearch : long
    {
        ALL = E.ALL,
    }
}
