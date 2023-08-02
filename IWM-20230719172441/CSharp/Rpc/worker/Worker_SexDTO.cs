using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_SexDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Worker_SexDTO() {}
        public Worker_SexDTO(Sex Sex)
        {
            this.Id = Sex.Id;
            this.Code = Sex.Code;
            this.Name = Sex.Name;
            this.Informations = Sex.Informations;
            this.Warnings = Sex.Warnings;
            this.Errors = Sex.Errors;
        }
    }

    public class Worker_SexFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public SexOrder OrderBy { get; set; }
    }
}