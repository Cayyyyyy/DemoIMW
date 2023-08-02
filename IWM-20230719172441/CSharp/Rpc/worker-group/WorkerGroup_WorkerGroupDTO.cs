using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroup_WorkerGroupDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public WorkerGroup_StatusDTO Status { get; set; }
        public WorkerGroup_WorkerGroupDTO() {}
        public WorkerGroup_WorkerGroupDTO(WorkerGroup WorkerGroup)
        {
            this.Id = WorkerGroup.Id;
            this.Code = WorkerGroup.Code;
            this.Name = WorkerGroup.Name;
            this.StatusId = WorkerGroup.StatusId;
            this.Status = WorkerGroup.Status == null ? null : new WorkerGroup_StatusDTO(WorkerGroup.Status);
            this.Informations = WorkerGroup.Informations;
            this.Warnings = WorkerGroup.Warnings;
            this.Errors = WorkerGroup.Errors;
        }
    }

    public class WorkerGroup_WorkerGroupFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public WorkerGroupOrder OrderBy { get; set; }
    }
}
