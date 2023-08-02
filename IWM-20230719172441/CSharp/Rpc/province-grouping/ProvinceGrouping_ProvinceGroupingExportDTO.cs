using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGrouping_ProvinceGroupingExportDTO : DataDTO
    {
        public long STT {get; set; }
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
        public ProvinceGrouping_ProvinceGroupingExportDTO() {}
        public ProvinceGrouping_ProvinceGroupingExportDTO(ProvinceGrouping ProvinceGrouping)
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
}
