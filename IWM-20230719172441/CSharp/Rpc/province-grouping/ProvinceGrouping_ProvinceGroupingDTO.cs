using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGrouping_ProvinceGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public long? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public long Level { get; set; }
        public string Path { get; set; }
        public ProvinceGrouping_ProvinceGroupingDTO Parent { get; set; }
        public ProvinceGrouping_StatusDTO Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ProvinceGrouping_ProvinceGroupingDTO() {}
        public ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping ProvinceGrouping)
        {
            this.Id = ProvinceGrouping.Id;
            this.Code = ProvinceGrouping.Code;
            this.Name = ProvinceGrouping.Name;
            this.StatusId = ProvinceGrouping.StatusId;
            this.ParentId = ProvinceGrouping.ParentId;
            this.HasChildren = ProvinceGrouping.HasChildren;
            this.Level = ProvinceGrouping.Level;
            this.Path = ProvinceGrouping.Path;
            this.Parent = ProvinceGrouping.Parent == null ? null : new ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping.Parent);
            this.Status = ProvinceGrouping.Status == null ? null : new ProvinceGrouping_StatusDTO(ProvinceGrouping.Status);
            this.RowId = ProvinceGrouping.RowId;
            this.CreatedAt = ProvinceGrouping.CreatedAt;
            this.UpdatedAt = ProvinceGrouping.UpdatedAt;
            this.Informations = ProvinceGrouping.Informations;
            this.Warnings = ProvinceGrouping.Warnings;
            this.Errors = ProvinceGrouping.Errors;
        }
    }

    public class ProvinceGrouping_ProvinceGroupingFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ParentId { get; set; }
        public LongFilter Level { get; set; }
        public StringFilter Path { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public ProvinceGroupingOrder OrderBy { get; set; }
    }
}
