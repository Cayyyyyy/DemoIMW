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
using IWM.Services.MUnitOfMeasureGroupingContent;
using File = IWM.Entities.File;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContentController : RpcController
    {
        private readonly IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController(
            IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureGroupingContentService = UnitOfMeasureGroupingContentService;
            this.CurrentContext = CurrentContext;
        }

        [Route(UnitOfMeasureGroupingContentRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = ConvertFilterDTOToFilterEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO);
            UnitOfMeasureGroupingContentFilter = await UnitOfMeasureGroupingContentService.ToFilter(UnitOfMeasureGroupingContentFilter);
            int count = await UnitOfMeasureGroupingContentService.Count(UnitOfMeasureGroupingContentFilter);
            return count;
        }

        [Route(UnitOfMeasureGroupingContentRoute.List), HttpPost]
        public async Task<ActionResult<List<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO>>> List([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = ConvertFilterDTOToFilterEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO);
            UnitOfMeasureGroupingContentFilter = await UnitOfMeasureGroupingContentService.ToFilter(UnitOfMeasureGroupingContentFilter);
            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await UnitOfMeasureGroupingContentService.List(UnitOfMeasureGroupingContentFilter);
            List<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO> UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTOs = UnitOfMeasureGroupingContents
                .Select(c => new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(c)).ToList();
            return UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTOs;
        }

        [Route(UnitOfMeasureGroupingContentRoute.Get), HttpPost]
        public async Task<ActionResult<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO>> Get([FromBody]UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id))
                return Forbid();

            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = await UnitOfMeasureGroupingContentService.Get(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id);
            return new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent);
        }

        [Route(UnitOfMeasureGroupingContentRoute.Create), HttpPost]
        public async Task<ActionResult<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO>> Create([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id))
                return Forbid();

            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = ConvertDTOToEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
            UnitOfMeasureGroupingContent = await UnitOfMeasureGroupingContentService.Create(UnitOfMeasureGroupingContent);
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO = new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent);
            if (UnitOfMeasureGroupingContent.IsValidated)
                return UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO;
            else
                return BadRequest(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
        }
        

        [Route(UnitOfMeasureGroupingContentRoute.Update), HttpPost]
        public async Task<ActionResult<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO>> Update([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            if (!await HasPermission(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id))
                return Forbid();

            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = ConvertDTOToEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
            UnitOfMeasureGroupingContent = await UnitOfMeasureGroupingContentService.Update(UnitOfMeasureGroupingContent);
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO = new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent);
            if (UnitOfMeasureGroupingContent.IsValidated)
                return UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO;
            else
                return BadRequest(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
        }

        [Route(UnitOfMeasureGroupingContentRoute.Delete), HttpPost]
        public async Task<ActionResult<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO>> Delete([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id))
                return Forbid();

            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = ConvertDTOToEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
            UnitOfMeasureGroupingContent = await UnitOfMeasureGroupingContentService.Delete(UnitOfMeasureGroupingContent);
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO = new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent);
            if (UnitOfMeasureGroupingContent.IsValidated)
                return UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO;
            else
                return BadRequest(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO);
        }
        
        [Route(UnitOfMeasureGroupingContentRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = new UnitOfMeasureGroupingContentFilter();
            UnitOfMeasureGroupingContentFilter = await UnitOfMeasureGroupingContentService.ToFilter(UnitOfMeasureGroupingContentFilter);
            UnitOfMeasureGroupingContentFilter.Id = new IdFilter { In = Ids };
            UnitOfMeasureGroupingContentFilter.Selects = UnitOfMeasureGroupingContentSelect.Id;
            UnitOfMeasureGroupingContentFilter.Skip = 0;
            UnitOfMeasureGroupingContentFilter.Take = int.MaxValue;

            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await UnitOfMeasureGroupingContentService.List(UnitOfMeasureGroupingContentFilter);
            UnitOfMeasureGroupingContents = await UnitOfMeasureGroupingContentService.BulkDelete(UnitOfMeasureGroupingContents);
            if (UnitOfMeasureGroupingContents.Any(x => !x.IsValidated))
                return BadRequest(UnitOfMeasureGroupingContents.Where(x => !x.IsValidated));
            return true;
        }
        

        private async Task<bool> HasPermission(long Id)
        {
            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = new UnitOfMeasureGroupingContentFilter();
            UnitOfMeasureGroupingContentFilter = await UnitOfMeasureGroupingContentService.ToFilter(UnitOfMeasureGroupingContentFilter);
            if (Id == 0)
            {

            }
            else
            {
                UnitOfMeasureGroupingContentFilter.Id = new IdFilter { Equal = Id };
                int count = await UnitOfMeasureGroupingContentService.Count(UnitOfMeasureGroupingContentFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private UnitOfMeasureGroupingContent ConvertDTOToEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO)
        {
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.TrimString();
            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = new UnitOfMeasureGroupingContent();
            UnitOfMeasureGroupingContent.Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Id;
            UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGroupingId;
            UnitOfMeasureGroupingContent.UnitOfMeasureId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureId;
            UnitOfMeasureGroupingContent.Factor = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.Factor;
            UnitOfMeasureGroupingContent.UnitOfMeasure = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure == null ? null : new UnitOfMeasure
            {
                Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.Id,
                Code = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.Code,
                Name = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.Name,
                Description = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.Description,
                StatusId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.StatusId,
                IsDecimal = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.IsDecimal,
                Used = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.Used,
                ErpCode = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasure.ErpCode,
            };
            UnitOfMeasureGroupingContent.UnitOfMeasureGrouping = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
            {
                Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.Id,
                Code = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.Code,
                Name = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.Name,
                Description = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.Description,
                UnitOfMeasureId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.UnitOfMeasureId,
                StatusId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.StatusId,
                Used = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentDTO.UnitOfMeasureGrouping.Used,
            };
            UnitOfMeasureGroupingContent.BaseLanguage = CurrentContext.Language;
            return UnitOfMeasureGroupingContent;
        }

        private UnitOfMeasureGroupingContentFilter ConvertFilterDTOToFilterEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.TrimString();
            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = new UnitOfMeasureGroupingContentFilter();
            UnitOfMeasureGroupingContentFilter.Selects = UnitOfMeasureGroupingContentSelect.ALL;
            UnitOfMeasureGroupingContentFilter.SearchBy = UnitOfMeasureGroupingContentSearch.ALL;
            UnitOfMeasureGroupingContentFilter.Skip = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Skip;
            UnitOfMeasureGroupingContentFilter.Take = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Take;
            UnitOfMeasureGroupingContentFilter.OrderBy = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.OrderBy;
            UnitOfMeasureGroupingContentFilter.OrderType = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.OrderType;

            UnitOfMeasureGroupingContentFilter.Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Id;
            UnitOfMeasureGroupingContentFilter.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.UnitOfMeasureGroupingId;
            UnitOfMeasureGroupingContentFilter.UnitOfMeasureId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.UnitOfMeasureId;
            UnitOfMeasureGroupingContentFilter.Factor = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Factor;
            return UnitOfMeasureGroupingContentFilter;
        }
    }
}

