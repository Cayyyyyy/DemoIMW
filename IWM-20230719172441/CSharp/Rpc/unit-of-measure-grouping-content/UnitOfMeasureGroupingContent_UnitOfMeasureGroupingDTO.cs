using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO() {}
        public UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            this.Id = UnitOfMeasureGrouping.Id;
            this.Code = UnitOfMeasureGrouping.Code;
            this.Name = UnitOfMeasureGrouping.Name;
            this.Description = UnitOfMeasureGrouping.Description;
            this.UnitOfMeasureId = UnitOfMeasureGrouping.UnitOfMeasureId;
            this.StatusId = UnitOfMeasureGrouping.StatusId;
            this.Used = UnitOfMeasureGrouping.Used;
            this.RowId = UnitOfMeasureGrouping.RowId;
            this.CreatedAt = UnitOfMeasureGrouping.CreatedAt;
            this.UpdatedAt = UnitOfMeasureGrouping.UpdatedAt;
            this.Informations = UnitOfMeasureGrouping.Informations;
            this.Warnings = UnitOfMeasureGrouping.Warnings;
            this.Errors = UnitOfMeasureGrouping.Errors;
        }
    }

    public class UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public IdFilter StatusId { get; set; }
        public UnitOfMeasureGroupingOrder OrderBy { get; set; }
    }
}