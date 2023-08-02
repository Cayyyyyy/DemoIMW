using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_DistrictDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long ProvinceId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Worker_DistrictDTO() {}
        public Worker_DistrictDTO(District District)
        {
            this.Id = District.Id;
            this.Code = District.Code;
            this.Name = District.Name;
            this.Priority = District.Priority;
            this.ProvinceId = District.ProvinceId;
            this.StatusId = District.StatusId;
            this.Used = District.Used;
            this.RowId = District.RowId;
            this.CreatedAt = District.CreatedAt;
            this.UpdatedAt = District.UpdatedAt;
            this.Informations = District.Informations;
            this.Warnings = District.Warnings;
            this.Errors = District.Errors;
        }
    }

    public class Worker_DistrictFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter StatusId { get; set; }
        public DistrictOrder OrderBy { get; set; }
    }
}