using TrueSight.Common;
using IWM.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using IWM.Entities;

namespace IWM.Rpc.worker
{
    public class Worker_WorkerDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentificationNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? SexId { get; set; }
        public long? WorkerGroupId { get; set; }
        public long? NationId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Worker_DistrictDTO District { get; set; }
        public Worker_NationDTO Nation { get; set; }
        public Worker_ProvinceDTO Province { get; set; }
        public Worker_SexDTO Sex { get; set; }
        public Worker_StatusDTO Status { get; set; }
        public Worker_WardDTO Ward { get; set; }
        public Worker_WorkerGroupDTO WorkerGroup { get; set; }
        public Worker_WorkerDTO() {}
        public Worker_WorkerDTO(Worker Worker)
        {
            this.Id = Worker.Id;
            this.Code = Worker.Code;
            this.Name = Worker.Name;
            this.StatusId = Worker.StatusId;
            this.Birthday = Worker.Birthday;
            this.Phone = Worker.Phone;
            this.CitizenIdentificationNumber = Worker.CitizenIdentificationNumber;
            this.Email = Worker.Email;
            this.Address = Worker.Address;
            this.SexId = Worker.SexId;
            this.WorkerGroupId = Worker.WorkerGroupId;
            this.NationId = Worker.NationId;
            this.ProvinceId = Worker.ProvinceId;
            this.DistrictId = Worker.DistrictId;
            this.WardId = Worker.WardId;
            this.Username = Worker.Username;
            this.Password = Worker.Password;
            this.District = Worker.District == null ? null : new Worker_DistrictDTO(Worker.District);
            this.Nation = Worker.Nation == null ? null : new Worker_NationDTO(Worker.Nation);
            this.Province = Worker.Province == null ? null : new Worker_ProvinceDTO(Worker.Province);
            this.Sex = Worker.Sex == null ? null : new Worker_SexDTO(Worker.Sex);
            this.Status = Worker.Status == null ? null : new Worker_StatusDTO(Worker.Status);
            this.Ward = Worker.Ward == null ? null : new Worker_WardDTO(Worker.Ward);
            this.WorkerGroup = Worker.WorkerGroup == null ? null : new Worker_WorkerGroupDTO(Worker.WorkerGroup);
            this.Informations = Worker.Informations;
            this.Warnings = Worker.Warnings;
            this.Errors = Worker.Errors;
        }
    }

    public class Worker_WorkerFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter Birthday { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter CitizenIdentificationNumber { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public IdFilter SexId { get; set; }
        public IdFilter WorkerGroupId { get; set; }
        public IdFilter NationId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter Password { get; set; }
        public WorkerOrder OrderBy { get; set; }
    }
}
