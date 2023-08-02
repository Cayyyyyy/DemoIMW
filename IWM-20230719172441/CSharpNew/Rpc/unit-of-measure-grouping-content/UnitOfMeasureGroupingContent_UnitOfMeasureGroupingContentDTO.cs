using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal? Factor { get; set; }
        public UnitOfMeasureGroupingContent_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public Guid RowId { get; set; }
        public UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO() { }
        public UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            this.Id = UnitOfMeasureGroupingContent.Id;
            this.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            this.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            this.Factor = UnitOfMeasureGroupingContent.Factor;
            this.UnitOfMeasure = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? null : new UnitOfMeasureGroupingContent_UnitOfMeasureDTO(UnitOfMeasureGroupingContent.UnitOfMeasure);
            this.UnitOfMeasureGrouping = UnitOfMeasureGroupingContent.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping);
            this.RowId = UnitOfMeasureGroupingContent.RowId;
            this.Informations = UnitOfMeasureGroupingContent.Informations;
            this.Warnings = UnitOfMeasureGroupingContent.Warnings;
            this.Errors = UnitOfMeasureGroupingContent.Errors;
        }
    }

    public class UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Factor { get; set; }
        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
    }
}
