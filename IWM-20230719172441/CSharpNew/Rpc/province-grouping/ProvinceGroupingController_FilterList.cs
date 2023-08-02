using TrueSight;
using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Entities;
using IWM.Services.MStatus;
using IWM.Services.MProvinceGrouping;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController_FilterList : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly IProvinceGroupingService ProvinceGroupingService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController_FilterList(
            IStatusService StatusService,
            IProvinceGroupingService ProvinceGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.ProvinceGroupingService = ProvinceGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProvinceGroupingRoute.FilterListProvinceGrouping), HttpPost]
        public async Task<List<ProvinceGrouping_ProvinceGroupingDTO>> FilterListProvinceGrouping([FromBody] ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter();
            ProvinceGroupingFilter.Skip = 0;
            ProvinceGroupingFilter.Take = int.MaxValue;
            ProvinceGroupingFilter.OrderBy = ProvinceGroupingOrder.Id;
            ProvinceGroupingFilter.OrderType = OrderType.ASC;
            ProvinceGroupingFilter.Selects = ProvinceGroupingSelect.ALL;
            ProvinceGroupingFilter.Id = ProvinceGrouping_ProvinceGroupingFilterDTO.Id;
            ProvinceGroupingFilter.Code = ProvinceGrouping_ProvinceGroupingFilterDTO.Code;
            ProvinceGroupingFilter.Name = ProvinceGrouping_ProvinceGroupingFilterDTO.Name;
            ProvinceGroupingFilter.StatusId = ProvinceGrouping_ProvinceGroupingFilterDTO.StatusId;
            ProvinceGroupingFilter.ParentId = ProvinceGrouping_ProvinceGroupingFilterDTO.ParentId;
            ProvinceGroupingFilter.Level = ProvinceGrouping_ProvinceGroupingFilterDTO.Level;
            ProvinceGroupingFilter.Path = ProvinceGrouping_ProvinceGroupingFilterDTO.Path;
            ProvinceGroupingFilter.TrimString();

            List<ProvinceGrouping> ProvinceGroupings = await ProvinceGroupingService.List(ProvinceGroupingFilter);
            List<ProvinceGrouping_ProvinceGroupingDTO> ProvinceGrouping_ProvinceGroupingDTOs = ProvinceGroupings
                .Select(x => new ProvinceGrouping_ProvinceGroupingDTO(x)).ToList();
            return ProvinceGrouping_ProvinceGroupingDTOs;
        }
        [Route(ProvinceGroupingRoute.FilterListStatus), HttpPost]
        public async Task<List<ProvinceGrouping_StatusDTO>> FilterListStatus([FromBody] ProvinceGrouping_StatusFilterDTO ProvinceGrouping_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = ProvinceGrouping_StatusFilterDTO.Id;
            StatusFilter.Code = ProvinceGrouping_StatusFilterDTO.Code;
            StatusFilter.Name = ProvinceGrouping_StatusFilterDTO.Name;
            StatusFilter.Color = ProvinceGrouping_StatusFilterDTO.Color;
            StatusFilter.TrimString();

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<ProvinceGrouping_StatusDTO> ProvinceGrouping_StatusDTOs = Statuses
                .Select(x => new ProvinceGrouping_StatusDTO(x)).ToList();
            return ProvinceGrouping_StatusDTOs;
        }

        [Route(ProvinceGroupingRoute.FilterListSelectOption), HttpPost]
        public async Task<List<SelectOption>> FilterListSelectOption()
        {
            return SelectOption.SelectOptionEnumList;
        }
    }
}