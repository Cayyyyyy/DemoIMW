using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContent_UnitOfMeasureDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public bool IsDecimal { get; set; }
        public bool Used { get; set; }
        public string ErpCode { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UnitOfMeasureGroupingContent_UnitOfMeasureDTO() { }
        public UnitOfMeasureGroupingContent_UnitOfMeasureDTO(UnitOfMeasure UnitOfMeasure)
        {
            this.Id = UnitOfMeasure.Id;
            this.Code = UnitOfMeasure.Code;
            this.Name = UnitOfMeasure.Name;
            this.Description = UnitOfMeasure.Description;
            this.StatusId = UnitOfMeasure.StatusId;
            this.IsDecimal = UnitOfMeasure.IsDecimal;
            this.Used = UnitOfMeasure.Used;
            this.ErpCode = UnitOfMeasure.ErpCode;
            this.RowId = UnitOfMeasure.RowId;
            this.CreatedAt = UnitOfMeasure.CreatedAt;
            this.UpdatedAt = UnitOfMeasure.UpdatedAt;
            this.Informations = UnitOfMeasure.Informations;
            this.Warnings = UnitOfMeasure.Warnings;
            this.Errors = UnitOfMeasure.Errors;
        }
    }

    public class UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter ErpCode { get; set; }
        public UnitOfMeasureOrder OrderBy { get; set; }
    }
}