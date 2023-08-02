using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_ProvinceDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Worker_ProvinceDTO() {}
        public Worker_ProvinceDTO(Province Province)
        {
            this.Id = Province.Id;
            this.Code = Province.Code;
            this.Name = Province.Name;
            this.Priority = Province.Priority;
            this.StatusId = Province.StatusId;
            this.Used = Province.Used;
            this.RowId = Province.RowId;
            this.CreatedAt = Province.CreatedAt;
            this.UpdatedAt = Province.UpdatedAt;
            this.Informations = Province.Informations;
            this.Warnings = Province.Warnings;
            this.Errors = Province.Errors;
        }
    }

    public class Worker_ProvinceFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public ProvinceOrder OrderBy { get; set; }
    }
}