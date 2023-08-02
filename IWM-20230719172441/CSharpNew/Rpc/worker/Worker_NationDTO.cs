using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_NationDTO : DataDTO
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
        public Worker_NationDTO() { }
        public Worker_NationDTO(Nation Nation)
        {
            this.Id = Nation.Id;
            this.Code = Nation.Code;
            this.Name = Nation.Name;
            this.Priority = Nation.Priority;
            this.StatusId = Nation.StatusId;
            this.Used = Nation.Used;
            this.RowId = Nation.RowId;
            this.CreatedAt = Nation.CreatedAt;
            this.UpdatedAt = Nation.UpdatedAt;
            this.Informations = Nation.Informations;
            this.Warnings = Nation.Warnings;
            this.Errors = Nation.Errors;
        }
    }

    public class Worker_NationFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public NationOrder OrderBy { get; set; }
    }
}