using TrueSight;
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
using IWM.Services.MWorkerGroup;
using File = IWM.Entities.File;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroupController : RpcController
    {
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController(
            IWorkerGroupService WorkerGroupService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkerGroupService = WorkerGroupService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerGroupRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerGroupFilter WorkerGroupFilter = ConvertFilterDTOToFilterEntity(WorkerGroup_WorkerGroupFilterDTO);
            WorkerGroupFilter = await WorkerGroupService.ToFilter(WorkerGroupFilter);
            int count = await WorkerGroupService.Count(WorkerGroupFilter);
            return count;
        }

        [Route(WorkerGroupRoute.List), HttpPost]
        public async Task<ActionResult<List<WorkerGroup_WorkerGroupDTO>>> List([FromBody] WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerGroupFilter WorkerGroupFilter = ConvertFilterDTOToFilterEntity(WorkerGroup_WorkerGroupFilterDTO);
            WorkerGroupFilter = await WorkerGroupService.ToFilter(WorkerGroupFilter);
            List<WorkerGroup> WorkerGroups = await WorkerGroupService.List(WorkerGroupFilter);
            List<WorkerGroup_WorkerGroupDTO> WorkerGroup_WorkerGroupDTOs = WorkerGroups
                .Select(c => new WorkerGroup_WorkerGroupDTO(c)).ToList();
            return WorkerGroup_WorkerGroupDTOs;
        }

        [Route(WorkerGroupRoute.Get), HttpPost]
        public async Task<ActionResult<WorkerGroup_WorkerGroupDTO>> Get([FromBody]WorkerGroup_WorkerGroupDTO WorkerGroup_WorkerGroupDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkerGroup_WorkerGroupDTO.Id))
                return Forbid();

            WorkerGroup WorkerGroup = await WorkerGroupService.Get(WorkerGroup_WorkerGroupDTO.Id);
            return new WorkerGroup_WorkerGroupDTO(WorkerGroup);
        }

        [Route(WorkerGroupRoute.Create), HttpPost]
        public async Task<ActionResult<WorkerGroup_WorkerGroupDTO>> Create([FromBody] WorkerGroup_WorkerGroupDTO WorkerGroup_WorkerGroupDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(WorkerGroup_WorkerGroupDTO.Id))
                return Forbid();

            WorkerGroup WorkerGroup = ConvertDTOToEntity(WorkerGroup_WorkerGroupDTO);
            WorkerGroup = await WorkerGroupService.Create(WorkerGroup);
            WorkerGroup_WorkerGroupDTO = new WorkerGroup_WorkerGroupDTO(WorkerGroup);
            if (WorkerGroup.IsValidated)
                return WorkerGroup_WorkerGroupDTO;
            else
                return BadRequest(WorkerGroup_WorkerGroupDTO);
        }
        

        [Route(WorkerGroupRoute.Update), HttpPost]
        public async Task<ActionResult<WorkerGroup_WorkerGroupDTO>> Update([FromBody] WorkerGroup_WorkerGroupDTO WorkerGroup_WorkerGroupDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(WorkerGroup_WorkerGroupDTO.Id))
                return Forbid();

            WorkerGroup WorkerGroup = ConvertDTOToEntity(WorkerGroup_WorkerGroupDTO);
            WorkerGroup = await WorkerGroupService.Update(WorkerGroup);
            WorkerGroup_WorkerGroupDTO = new WorkerGroup_WorkerGroupDTO(WorkerGroup);
            if (WorkerGroup.IsValidated)
                return WorkerGroup_WorkerGroupDTO;
            else
                return BadRequest(WorkerGroup_WorkerGroupDTO);
        }

        [Route(WorkerGroupRoute.Delete), HttpPost]
        public async Task<ActionResult<WorkerGroup_WorkerGroupDTO>> Delete([FromBody] WorkerGroup_WorkerGroupDTO WorkerGroup_WorkerGroupDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkerGroup_WorkerGroupDTO.Id))
                return Forbid();

            WorkerGroup WorkerGroup = ConvertDTOToEntity(WorkerGroup_WorkerGroupDTO);
            WorkerGroup = await WorkerGroupService.Delete(WorkerGroup);
            WorkerGroup_WorkerGroupDTO = new WorkerGroup_WorkerGroupDTO(WorkerGroup);
            if (WorkerGroup.IsValidated)
                return WorkerGroup_WorkerGroupDTO;
            else
                return BadRequest(WorkerGroup_WorkerGroupDTO);
        }
        
        [Route(WorkerGroupRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter();
            WorkerGroupFilter = await WorkerGroupService.ToFilter(WorkerGroupFilter);
            WorkerGroupFilter.Id = new IdFilter { In = Ids };
            WorkerGroupFilter.Selects = WorkerGroupSelect.Id;
            WorkerGroupFilter.Skip = 0;
            WorkerGroupFilter.Take = int.MaxValue;

            List<WorkerGroup> WorkerGroups = await WorkerGroupService.List(WorkerGroupFilter);
            WorkerGroups = await WorkerGroupService.BulkDelete(WorkerGroups);
            if (WorkerGroups.Any(x => !x.IsValidated))
                return BadRequest(WorkerGroups.Where(x => !x.IsValidated));
            return true;
        }
        

        private async Task<bool> HasPermission(long Id)
        {
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter();
            WorkerGroupFilter = await WorkerGroupService.ToFilter(WorkerGroupFilter);
            if (Id == 0)
            {

            }
            else
            {
                WorkerGroupFilter.Id = new IdFilter { Equal = Id };
                int count = await WorkerGroupService.Count(WorkerGroupFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private WorkerGroup ConvertDTOToEntity(WorkerGroup_WorkerGroupDTO WorkerGroup_WorkerGroupDTO)
        {
            WorkerGroup_WorkerGroupDTO.TrimString();
            WorkerGroup WorkerGroup = new WorkerGroup();
            WorkerGroup.Id = WorkerGroup_WorkerGroupDTO.Id;
            WorkerGroup.Code = WorkerGroup_WorkerGroupDTO.Code;
            WorkerGroup.Name = WorkerGroup_WorkerGroupDTO.Name;
            WorkerGroup.StatusId = WorkerGroup_WorkerGroupDTO.StatusId;
            WorkerGroup.Status = WorkerGroup_WorkerGroupDTO.Status == null ? null : new Status
            {
                Id = WorkerGroup_WorkerGroupDTO.Status.Id,
                Code = WorkerGroup_WorkerGroupDTO.Status.Code,
                Name = WorkerGroup_WorkerGroupDTO.Status.Name,
                Color = WorkerGroup_WorkerGroupDTO.Status.Color,
            };
            WorkerGroup.BaseLanguage = CurrentContext.Language;
            return WorkerGroup;
        }

        private WorkerGroupFilter ConvertFilterDTOToFilterEntity(WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            WorkerGroup_WorkerGroupFilterDTO.TrimString();
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter();
            WorkerGroupFilter.Selects = WorkerGroupSelect.ALL;
            WorkerGroupFilter.SearchBy = WorkerGroupSearch.ALL;
            WorkerGroupFilter.Skip = WorkerGroup_WorkerGroupFilterDTO.Skip;
            WorkerGroupFilter.Take = WorkerGroup_WorkerGroupFilterDTO.Take;
            WorkerGroupFilter.OrderBy = WorkerGroup_WorkerGroupFilterDTO.OrderBy;
            WorkerGroupFilter.OrderType = WorkerGroup_WorkerGroupFilterDTO.OrderType;

            WorkerGroupFilter.Id = WorkerGroup_WorkerGroupFilterDTO.Id;
            WorkerGroupFilter.Code = WorkerGroup_WorkerGroupFilterDTO.Code;
            WorkerGroupFilter.Name = WorkerGroup_WorkerGroupFilterDTO.Name;
            WorkerGroupFilter.StatusId = WorkerGroup_WorkerGroupFilterDTO.StatusId;
            WorkerGroupFilter.SearchBy = WorkerGroupSearch.Code | WorkerGroupSearch.Name;
            WorkerGroupFilter.Search = WorkerGroup_WorkerGroupFilterDTO.Search;
            return WorkerGroupFilter;
        }
    }
}

