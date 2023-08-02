using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroup_StatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public WorkerGroup_StatusDTO() { }
        public WorkerGroup_StatusDTO(Status Status)
        {
            this.Id = Status.Id;
            this.Code = Status.Code;
            this.Name = Status.Name;
            this.Color = Status.Color;
            this.Informations = Status.Informations;
            this.Warnings = Status.Warnings;
            this.Errors = Status.Errors;
        }
    }

    public class WorkerGroup_StatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Color { get; set; }
        public StatusOrder OrderBy { get; set; }
    }
}