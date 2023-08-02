using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Entities;
using IWM.Services.MUnitOfMeasure;
using IWM.Services.MUnitOfMeasureGrouping;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContentController_SingleList : RpcController 
    {
        private readonly IUnitOfMeasureService UnitOfMeasureService;
        private readonly IUnitOfMeasureGroupingService UnitOfMeasureGroupingService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController_SingleList(
            IUnitOfMeasureService UnitOfMeasureService,
            IUnitOfMeasureGroupingService UnitOfMeasureGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.UnitOfMeasureGroupingService = UnitOfMeasureGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(UnitOfMeasureGroupingContentRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<UnitOfMeasureGroupingContent_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter();
            UnitOfMeasureFilter.Skip = 0;
            UnitOfMeasureFilter.Take = 20;
            UnitOfMeasureFilter.OrderBy = UnitOfMeasureOrder.Id;
            UnitOfMeasureFilter.OrderType = OrderType.ASC;
            UnitOfMeasureFilter.Selects = UnitOfMeasureSelect.ALL;
            UnitOfMeasureFilter.Id = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.Id;
            UnitOfMeasureFilter.Code = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.Code;
            UnitOfMeasureFilter.Name = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.Name;
            UnitOfMeasureFilter.Description = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.Description;
            UnitOfMeasureFilter.StatusId = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.StatusId;
            UnitOfMeasureFilter.ErpCode = UnitOfMeasureGroupingContent_UnitOfMeasureFilterDTO.ErpCode;
            UnitOfMeasureFilter.TrimString();
            UnitOfMeasureFilter.StatusId = new IdFilter{ Equal = 1 };

            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            List<UnitOfMeasureGroupingContent_UnitOfMeasureDTO> UnitOfMeasureGroupingContent_UnitOfMeasureDTOs = UnitOfMeasures
                .Select(x => new UnitOfMeasureGroupingContent_UnitOfMeasureDTO(x)).ToList();
            return UnitOfMeasureGroupingContent_UnitOfMeasureDTOs;
        }
        [Route(UnitOfMeasureGroupingContentRoute.SingleListUnitOfMeasureGrouping), HttpPost]
        public async Task<List<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO>> SingleListUnitOfMeasureGrouping([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter = new UnitOfMeasureGroupingFilter();
            UnitOfMeasureGroupingFilter.Skip = 0;
            UnitOfMeasureGroupingFilter.Take = 20;
            UnitOfMeasureGroupingFilter.OrderBy = UnitOfMeasureGroupingOrder.Id;
            UnitOfMeasureGroupingFilter.OrderType = OrderType.ASC;
            UnitOfMeasureGroupingFilter.Selects = UnitOfMeasureGroupingSelect.ALL;
            UnitOfMeasureGroupingFilter.Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.Id;
            UnitOfMeasureGroupingFilter.Code = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.Code;
            UnitOfMeasureGroupingFilter.Name = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.Name;
            UnitOfMeasureGroupingFilter.Description = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.Description;
            UnitOfMeasureGroupingFilter.UnitOfMeasureId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.UnitOfMeasureId;
            UnitOfMeasureGroupingFilter.StatusId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingFilterDTO.StatusId;
            UnitOfMeasureGroupingFilter.TrimString();
            UnitOfMeasureGroupingFilter.StatusId = new IdFilter{ Equal = 1 };

            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await UnitOfMeasureGroupingService.List(UnitOfMeasureGroupingFilter);
            List<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO> UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTOs = UnitOfMeasureGroupings
                .Select(x => new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTO(x)).ToList();
            return UnitOfMeasureGroupingContent_UnitOfMeasureGroupingDTOs;
        }
    }
}

