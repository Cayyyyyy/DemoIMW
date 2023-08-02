using TrueSight;
using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Entities;
using IWM.Services.MStatus;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroupController_FilterList : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController_FilterList(
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerGroupRoute.FilterListStatus), HttpPost]
        public async Task<List<WorkerGroup_StatusDTO>> FilterListStatus([FromBody] WorkerGroup_StatusFilterDTO WorkerGroup_StatusFilterDTO)
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
            StatusFilter.Id = WorkerGroup_StatusFilterDTO.Id;
            StatusFilter.Code = WorkerGroup_StatusFilterDTO.Code;
            StatusFilter.Name = WorkerGroup_StatusFilterDTO.Name;
            StatusFilter.Color = WorkerGroup_StatusFilterDTO.Color;
            StatusFilter.TrimString();

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkerGroup_StatusDTO> WorkerGroup_StatusDTOs = Statuses
                .Select(x => new WorkerGroup_StatusDTO(x)).ToList();
            return WorkerGroup_StatusDTOs;
        }

        [Route(WorkerGroupRoute.FilterListSelectOption), HttpPost]
        public async Task<List<SelectOption>> FilterListSelectOption()
        {
            return SelectOption.SelectOptionEnumList;
        }
    }
}