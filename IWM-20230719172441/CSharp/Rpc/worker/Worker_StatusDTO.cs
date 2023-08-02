using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_StatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Worker_StatusDTO() {}
        public Worker_StatusDTO(Status Status)
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

    public class Worker_StatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Color { get; set; }
        public StatusOrder OrderBy { get; set; }
    }
}