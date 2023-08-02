using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NGS.Templater;
using System.IO;
using IWM.Entities;
using IWM.Services.MWorker;
using File = IWM.Entities.File;

namespace IWM.Rpc.worker
{
    public class WorkerController : RpcController
    {
        private readonly IWorkerService WorkerService;
        private readonly ICurrentContext CurrentContext;
        public WorkerController(
            IWorkerService WorkerService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkerService = WorkerService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerFilter WorkerFilter = ConvertFilterDTOToFilterEntity(Worker_WorkerFilterDTO);
            WorkerFilter = await WorkerService.ToFilter(WorkerFilter);
            int count = await WorkerService.Count(WorkerFilter);
            return count;
        }

        [Route(WorkerRoute.List), HttpPost]
        public async Task<ActionResult<List<Worker_WorkerDTO>>> List([FromBody] Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerFilter WorkerFilter = ConvertFilterDTOToFilterEntity(Worker_WorkerFilterDTO);
            WorkerFilter = await WorkerService.ToFilter(WorkerFilter);
            List<Worker> Workers = await WorkerService.List(WorkerFilter);
            List<Worker_WorkerDTO> Worker_WorkerDTOs = Workers
                .Select(c => new Worker_WorkerDTO(c)).ToList();
            return Worker_WorkerDTOs;
        }

        [Route(WorkerRoute.Get), HttpPost]
        public async Task<ActionResult<Worker_WorkerDTO>> Get([FromBody]Worker_WorkerDTO Worker_WorkerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Worker_WorkerDTO.Id))
                return Forbid();

            Worker Worker = await WorkerService.Get(Worker_WorkerDTO.Id);
            return new Worker_WorkerDTO(Worker);
        }

        [Route(WorkerRoute.Create), HttpPost]
        public async Task<ActionResult<Worker_WorkerDTO>> Create([FromBody] Worker_WorkerDTO Worker_WorkerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(Worker_WorkerDTO.Id))
                return Forbid();

            Worker Worker = ConvertDTOToEntity(Worker_WorkerDTO);
            Worker = await WorkerService.Create(Worker);
            Worker_WorkerDTO = new Worker_WorkerDTO(Worker);
            if (Worker.IsValidated)
                return Worker_WorkerDTO;
            else
                return BadRequest(Worker_WorkerDTO);
        }
        

        [Route(WorkerRoute.Update), HttpPost]
        public async Task<ActionResult<Worker_WorkerDTO>> Update([FromBody] Worker_WorkerDTO Worker_WorkerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(Worker_WorkerDTO.Id))
                return Forbid();

            Worker Worker = ConvertDTOToEntity(Worker_WorkerDTO);
            Worker = await WorkerService.Update(Worker);
            Worker_WorkerDTO = new Worker_WorkerDTO(Worker);
            if (Worker.IsValidated)
                return Worker_WorkerDTO;
            else
                return BadRequest(Worker_WorkerDTO);
        }

        [Route(WorkerRoute.Delete), HttpPost]
        public async Task<ActionResult<Worker_WorkerDTO>> Delete([FromBody] Worker_WorkerDTO Worker_WorkerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Worker_WorkerDTO.Id))
                return Forbid();

            Worker Worker = ConvertDTOToEntity(Worker_WorkerDTO);
            Worker = await WorkerService.Delete(Worker);
            Worker_WorkerDTO = new Worker_WorkerDTO(Worker);
            if (Worker.IsValidated)
                return Worker_WorkerDTO;
            else
                return BadRequest(Worker_WorkerDTO);
        }
        
        [Route(WorkerRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerFilter WorkerFilter = new WorkerFilter();
            WorkerFilter = await WorkerService.ToFilter(WorkerFilter);
            WorkerFilter.Id = new IdFilter { In = Ids };
            WorkerFilter.Selects = WorkerSelect.Id;
            WorkerFilter.Skip = 0;
            WorkerFilter.Take = int.MaxValue;

            List<Worker> Workers = await WorkerService.List(WorkerFilter);
            Workers = await WorkerService.BulkDelete(Workers);
            if (Workers.Any(x => !x.IsValidated))
                return BadRequest(Workers.Where(x => !x.IsValidated));
            return true;
        }
        

        private async Task<bool> HasPermission(long Id)
        {
            WorkerFilter WorkerFilter = new WorkerFilter();
            WorkerFilter = await WorkerService.ToFilter(WorkerFilter);
            if (Id == 0)
            {

            }
            else
            {
                WorkerFilter.Id = new IdFilter { Equal = Id };
                int count = await WorkerService.Count(WorkerFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Worker ConvertDTOToEntity(Worker_WorkerDTO Worker_WorkerDTO)
        {
            Worker_WorkerDTO.TrimString();
            Worker Worker = new Worker();
            Worker.Id = Worker_WorkerDTO.Id;
            Worker.Code = Worker_WorkerDTO.Code;
            Worker.Name = Worker_WorkerDTO.Name;
            Worker.StatusId = Worker_WorkerDTO.StatusId;
            Worker.Birthday = Worker_WorkerDTO.Birthday;
            Worker.Phone = Worker_WorkerDTO.Phone;
            Worker.CitizenIdentificationNumber = Worker_WorkerDTO.CitizenIdentificationNumber;
            Worker.Email = Worker_WorkerDTO.Email;
            Worker.Address = Worker_WorkerDTO.Address;
            Worker.SexId = Worker_WorkerDTO.SexId;
            Worker.WorkerGroupId = Worker_WorkerDTO.WorkerGroupId;
            Worker.NationId = Worker_WorkerDTO.NationId;
            Worker.ProvinceId = Worker_WorkerDTO.ProvinceId;
            Worker.DistrictId = Worker_WorkerDTO.DistrictId;
            Worker.WardId = Worker_WorkerDTO.WardId;
            Worker.Username = Worker_WorkerDTO.Username;
            Worker.Password = Worker_WorkerDTO.Password;
            Worker.District = Worker_WorkerDTO.District == null ? null : new District
            {
                Id = Worker_WorkerDTO.District.Id,
                Code = Worker_WorkerDTO.District.Code,
                Name = Worker_WorkerDTO.District.Name,
                Priority = Worker_WorkerDTO.District.Priority,
                ProvinceId = Worker_WorkerDTO.District.ProvinceId,
                StatusId = Worker_WorkerDTO.District.StatusId,
                Used = Worker_WorkerDTO.District.Used,
            };
            Worker.Nation = Worker_WorkerDTO.Nation == null ? null : new Nation
            {
                Id = Worker_WorkerDTO.Nation.Id,
                Code = Worker_WorkerDTO.Nation.Code,
                Name = Worker_WorkerDTO.Nation.Name,
                Priority = Worker_WorkerDTO.Nation.Priority,
                StatusId = Worker_WorkerDTO.Nation.StatusId,
                Used = Worker_WorkerDTO.Nation.Used,
            };
            Worker.Province = Worker_WorkerDTO.Province == null ? null : new Province
            {
                Id = Worker_WorkerDTO.Province.Id,
                Code = Worker_WorkerDTO.Province.Code,
                Name = Worker_WorkerDTO.Province.Name,
                Priority = Worker_WorkerDTO.Province.Priority,
                StatusId = Worker_WorkerDTO.Province.StatusId,
                Used = Worker_WorkerDTO.Province.Used,
            };
            Worker.Sex = Worker_WorkerDTO.Sex == null ? null : new Sex
            {
                Id = Worker_WorkerDTO.Sex.Id,
                Code = Worker_WorkerDTO.Sex.Code,
                Name = Worker_WorkerDTO.Sex.Name,
            };
            Worker.Status = Worker_WorkerDTO.Status == null ? null : new Status
            {
                Id = Worker_WorkerDTO.Status.Id,
                Code = Worker_WorkerDTO.Status.Code,
                Name = Worker_WorkerDTO.Status.Name,
                Color = Worker_WorkerDTO.Status.Color,
            };
            Worker.Ward = Worker_WorkerDTO.Ward == null ? null : new Ward
            {
                Id = Worker_WorkerDTO.Ward.Id,
                Code = Worker_WorkerDTO.Ward.Code,
                Name = Worker_WorkerDTO.Ward.Name,
                Priority = Worker_WorkerDTO.Ward.Priority,
                DistrictId = Worker_WorkerDTO.Ward.DistrictId,
                StatusId = Worker_WorkerDTO.Ward.StatusId,
                Used = Worker_WorkerDTO.Ward.Used,
            };
            Worker.WorkerGroup = Worker_WorkerDTO.WorkerGroup == null ? null : new WorkerGroup
            {
                Id = Worker_WorkerDTO.WorkerGroup.Id,
                Code = Worker_WorkerDTO.WorkerGroup.Code,
                Name = Worker_WorkerDTO.WorkerGroup.Name,
                StatusId = Worker_WorkerDTO.WorkerGroup.StatusId,
            };
            Worker.BaseLanguage = CurrentContext.Language;
            return Worker;
        }

        private WorkerFilter ConvertFilterDTOToFilterEntity(Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            Worker_WorkerFilterDTO.TrimString();
            WorkerFilter WorkerFilter = new WorkerFilter();
            WorkerFilter.Selects = WorkerSelect.ALL;
            WorkerFilter.SearchBy = WorkerSearch.ALL;
            WorkerFilter.Skip = Worker_WorkerFilterDTO.Skip;
            WorkerFilter.Take = Worker_WorkerFilterDTO.Take;
            WorkerFilter.OrderBy = Worker_WorkerFilterDTO.OrderBy;
            WorkerFilter.OrderType = Worker_WorkerFilterDTO.OrderType;

            WorkerFilter.Id = Worker_WorkerFilterDTO.Id;
            WorkerFilter.Code = Worker_WorkerFilterDTO.Code;
            WorkerFilter.Name = Worker_WorkerFilterDTO.Name;
            WorkerFilter.StatusId = Worker_WorkerFilterDTO.StatusId;
            WorkerFilter.Birthday = Worker_WorkerFilterDTO.Birthday;
            WorkerFilter.Phone = Worker_WorkerFilterDTO.Phone;
            WorkerFilter.CitizenIdentificationNumber = Worker_WorkerFilterDTO.CitizenIdentificationNumber;
            WorkerFilter.Email = Worker_WorkerFilterDTO.Email;
            WorkerFilter.Address = Worker_WorkerFilterDTO.Address;
            WorkerFilter.SexId = Worker_WorkerFilterDTO.SexId;
            WorkerFilter.WorkerGroupId = Worker_WorkerFilterDTO.WorkerGroupId;
            WorkerFilter.NationId = Worker_WorkerFilterDTO.NationId;
            WorkerFilter.ProvinceId = Worker_WorkerFilterDTO.ProvinceId;
            WorkerFilter.DistrictId = Worker_WorkerFilterDTO.DistrictId;
            WorkerFilter.WardId = Worker_WorkerFilterDTO.WardId;
            WorkerFilter.Username = Worker_WorkerFilterDTO.Username;
            WorkerFilter.Password = Worker_WorkerFilterDTO.Password;
            WorkerFilter.SearchBy = WorkerSearch.Code | WorkerSearch.Name;
            WorkerFilter.Search = Worker_WorkerFilterDTO.Search;
            return WorkerFilter;
        }
    }
}

