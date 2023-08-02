using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_WardDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long DistrictId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Worker_WardDTO() { }
        public Worker_WardDTO(Ward Ward)
        {
            this.Id = Ward.Id;
            this.Code = Ward.Code;
            this.Name = Ward.Name;
            this.Priority = Ward.Priority;
            this.DistrictId = Ward.DistrictId;
            this.StatusId = Ward.StatusId;
            this.Used = Ward.Used;
            this.RowId = Ward.RowId;
            this.CreatedAt = Ward.CreatedAt;
            this.UpdatedAt = Ward.UpdatedAt;
            this.Informations = Ward.Informations;
            this.Warnings = Ward.Warnings;
            this.Errors = Ward.Errors;
        }
    }

    public class Worker_WardFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter StatusId { get; set; }
        public WardOrder OrderBy { get; set; }
    }
}