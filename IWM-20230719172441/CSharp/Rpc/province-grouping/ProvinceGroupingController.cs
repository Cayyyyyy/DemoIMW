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
using IWM.Services.MProvinceGrouping;
using File = IWM.Entities.File;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController : RpcController
    {
        private readonly IProvinceGroupingService ProvinceGroupingService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController(
            IProvinceGroupingService ProvinceGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.ProvinceGroupingService = ProvinceGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProvinceGroupingRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceGroupingFilter ProvinceGroupingFilter = ConvertFilterDTOToFilterEntity(ProvinceGrouping_ProvinceGroupingFilterDTO);
            ProvinceGroupingFilter = await ProvinceGroupingService.ToFilter(ProvinceGroupingFilter);
            int count = await ProvinceGroupingService.Count(ProvinceGroupingFilter);
            return count;
        }

        [Route(ProvinceGroupingRoute.List), HttpPost]
        public async Task<ActionResult<List<ProvinceGrouping_ProvinceGroupingDTO>>> List([FromBody] ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceGroupingFilter ProvinceGroupingFilter = ConvertFilterDTOToFilterEntity(ProvinceGrouping_ProvinceGroupingFilterDTO);
            ProvinceGroupingFilter = await ProvinceGroupingService.ToFilter(ProvinceGroupingFilter);
            List<ProvinceGrouping> ProvinceGroupings = await ProvinceGroupingService.List(ProvinceGroupingFilter);
            List<ProvinceGrouping_ProvinceGroupingDTO> ProvinceGrouping_ProvinceGroupingDTOs = ProvinceGroupings
                .Select(c => new ProvinceGrouping_ProvinceGroupingDTO(c)).ToList();
            return ProvinceGrouping_ProvinceGroupingDTOs;
        }

        [Route(ProvinceGroupingRoute.Get), HttpPost]
        public async Task<ActionResult<ProvinceGrouping_ProvinceGroupingDTO>> Get([FromBody]ProvinceGrouping_ProvinceGroupingDTO ProvinceGrouping_ProvinceGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProvinceGrouping_ProvinceGroupingDTO.Id))
                return Forbid();

            ProvinceGrouping ProvinceGrouping = await ProvinceGroupingService.Get(ProvinceGrouping_ProvinceGroupingDTO.Id);
            return new ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping);
        }

        [Route(ProvinceGroupingRoute.Create), HttpPost]
        public async Task<ActionResult<ProvinceGrouping_ProvinceGroupingDTO>> Create([FromBody] ProvinceGrouping_ProvinceGroupingDTO ProvinceGrouping_ProvinceGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(ProvinceGrouping_ProvinceGroupingDTO.Id))
                return Forbid();

            ProvinceGrouping ProvinceGrouping = ConvertDTOToEntity(ProvinceGrouping_ProvinceGroupingDTO);
            ProvinceGrouping = await ProvinceGroupingService.Create(ProvinceGrouping);
            ProvinceGrouping_ProvinceGroupingDTO = new ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping);
            if (ProvinceGrouping.IsValidated)
                return ProvinceGrouping_ProvinceGroupingDTO;
            else
                return BadRequest(ProvinceGrouping_ProvinceGroupingDTO);
        }
        

        [Route(ProvinceGroupingRoute.Update), HttpPost]
        public async Task<ActionResult<ProvinceGrouping_ProvinceGroupingDTO>> Update([FromBody] ProvinceGrouping_ProvinceGroupingDTO ProvinceGrouping_ProvinceGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(ProvinceGrouping_ProvinceGroupingDTO.Id))
                return Forbid();

            ProvinceGrouping ProvinceGrouping = ConvertDTOToEntity(ProvinceGrouping_ProvinceGroupingDTO);
            ProvinceGrouping = await ProvinceGroupingService.Update(ProvinceGrouping);
            ProvinceGrouping_ProvinceGroupingDTO = new ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping);
            if (ProvinceGrouping.IsValidated)
                return ProvinceGrouping_ProvinceGroupingDTO;
            else
                return BadRequest(ProvinceGrouping_ProvinceGroupingDTO);
        }

        [Route(ProvinceGroupingRoute.Delete), HttpPost]
        public async Task<ActionResult<ProvinceGrouping_ProvinceGroupingDTO>> Delete([FromBody] ProvinceGrouping_ProvinceGroupingDTO ProvinceGrouping_ProvinceGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProvinceGrouping_ProvinceGroupingDTO.Id))
                return Forbid();

            ProvinceGrouping ProvinceGrouping = ConvertDTOToEntity(ProvinceGrouping_ProvinceGroupingDTO);
            ProvinceGrouping = await ProvinceGroupingService.Delete(ProvinceGrouping);
            ProvinceGrouping_ProvinceGroupingDTO = new ProvinceGrouping_ProvinceGroupingDTO(ProvinceGrouping);
            if (ProvinceGrouping.IsValidated)
                return ProvinceGrouping_ProvinceGroupingDTO;
            else
                return BadRequest(ProvinceGrouping_ProvinceGroupingDTO);
        }
        
        [Route(ProvinceGroupingRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter();
            ProvinceGroupingFilter = await ProvinceGroupingService.ToFilter(ProvinceGroupingFilter);
            ProvinceGroupingFilter.Id = new IdFilter { In = Ids };
            ProvinceGroupingFilter.Selects = ProvinceGroupingSelect.Id;
            ProvinceGroupingFilter.Skip = 0;
            ProvinceGroupingFilter.Take = int.MaxValue;

            List<ProvinceGrouping> ProvinceGroupings = await ProvinceGroupingService.List(ProvinceGroupingFilter);
            ProvinceGroupings = await ProvinceGroupingService.BulkDelete(ProvinceGroupings);
            if (ProvinceGroupings.Any(x => !x.IsValidated))
                return BadRequest(ProvinceGroupings.Where(x => !x.IsValidated));
            return true;
        }
        

        private async Task<bool> HasPermission(long Id)
        {
            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter();
            ProvinceGroupingFilter = await ProvinceGroupingService.ToFilter(ProvinceGroupingFilter);
            if (Id == 0)
            {

            }
            else
            {
                ProvinceGroupingFilter.Id = new IdFilter { Equal = Id };
                int count = await ProvinceGroupingService.Count(ProvinceGroupingFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ProvinceGrouping ConvertDTOToEntity(ProvinceGrouping_ProvinceGroupingDTO ProvinceGrouping_ProvinceGroupingDTO)
        {
            ProvinceGrouping_ProvinceGroupingDTO.TrimString();
            ProvinceGrouping ProvinceGrouping = new ProvinceGrouping();
            ProvinceGrouping.Id = ProvinceGrouping_ProvinceGroupingDTO.Id;
            ProvinceGrouping.Code = ProvinceGrouping_ProvinceGroupingDTO.Code;
            ProvinceGrouping.Name = ProvinceGrouping_ProvinceGroupingDTO.Name;
            ProvinceGrouping.StatusId = ProvinceGrouping_ProvinceGroupingDTO.StatusId;
            ProvinceGrouping.ParentId = ProvinceGrouping_ProvinceGroupingDTO.ParentId;
            ProvinceGrouping.HasChildren = ProvinceGrouping_ProvinceGroupingDTO.HasChildren;
            ProvinceGrouping.Level = ProvinceGrouping_ProvinceGroupingDTO.Level;
            ProvinceGrouping.Path = ProvinceGrouping_ProvinceGroupingDTO.Path;
            ProvinceGrouping.Parent = ProvinceGrouping_ProvinceGroupingDTO.Parent == null ? null : new ProvinceGrouping
            {
                Id = ProvinceGrouping_ProvinceGroupingDTO.Parent.Id,
                Code = ProvinceGrouping_ProvinceGroupingDTO.Parent.Code,
                Name = ProvinceGrouping_ProvinceGroupingDTO.Parent.Name,
                StatusId = ProvinceGrouping_ProvinceGroupingDTO.Parent.StatusId,
                ParentId = ProvinceGrouping_ProvinceGroupingDTO.Parent.ParentId,
                HasChildren = ProvinceGrouping_ProvinceGroupingDTO.Parent.HasChildren,
                Level = ProvinceGrouping_ProvinceGroupingDTO.Parent.Level,
                Path = ProvinceGrouping_ProvinceGroupingDTO.Parent.Path,
            };
            ProvinceGrouping.Status = ProvinceGrouping_ProvinceGroupingDTO.Status == null ? null : new Status
            {
                Id = ProvinceGrouping_ProvinceGroupingDTO.Status.Id,
                Code = ProvinceGrouping_ProvinceGroupingDTO.Status.Code,
                Name = ProvinceGrouping_ProvinceGroupingDTO.Status.Name,
                Color = ProvinceGrouping_ProvinceGroupingDTO.Status.Color,
            };
            ProvinceGrouping.BaseLanguage = CurrentContext.Language;
            return ProvinceGrouping;
        }

        private ProvinceGroupingFilter ConvertFilterDTOToFilterEntity(ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            ProvinceGrouping_ProvinceGroupingFilterDTO.TrimString();
            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter();
            ProvinceGroupingFilter.Selects = ProvinceGroupingSelect.ALL;
            ProvinceGroupingFilter.SearchBy = ProvinceGroupingSearch.ALL;
            ProvinceGroupingFilter.Skip = 0;
            ProvinceGroupingFilter.Take = 99999;
            ProvinceGroupingFilter.OrderBy = ProvinceGrouping_ProvinceGroupingFilterDTO.OrderBy;
            ProvinceGroupingFilter.OrderType = ProvinceGrouping_ProvinceGroupingFilterDTO.OrderType;

            ProvinceGroupingFilter.Id = ProvinceGrouping_ProvinceGroupingFilterDTO.Id;
            ProvinceGroupingFilter.Code = ProvinceGrouping_ProvinceGroupingFilterDTO.Code;
            ProvinceGroupingFilter.Name = ProvinceGrouping_ProvinceGroupingFilterDTO.Name;
            ProvinceGroupingFilter.StatusId = ProvinceGrouping_ProvinceGroupingFilterDTO.StatusId;
            ProvinceGroupingFilter.ParentId = ProvinceGrouping_ProvinceGroupingFilterDTO.ParentId;
            ProvinceGroupingFilter.Level = ProvinceGrouping_ProvinceGroupingFilterDTO.Level;
            ProvinceGroupingFilter.Path = ProvinceGrouping_ProvinceGroupingFilterDTO.Path;
            ProvinceGroupingFilter.CreatedAt = ProvinceGrouping_ProvinceGroupingFilterDTO.CreatedAt;
            ProvinceGroupingFilter.UpdatedAt = ProvinceGrouping_ProvinceGroupingFilterDTO.UpdatedAt;
            ProvinceGroupingFilter.SearchBy = ProvinceGroupingSearch.Code | ProvinceGroupingSearch.Name;
            ProvinceGroupingFilter.Search = ProvinceGrouping_ProvinceGroupingFilterDTO.Search;
            return ProvinceGroupingFilter;
        }
    }
}

