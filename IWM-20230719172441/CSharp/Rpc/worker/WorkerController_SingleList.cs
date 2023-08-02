using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Entities;
using IWM.Services.MDistrict;
using IWM.Services.MNation;
using IWM.Services.MProvince;
using IWM.Services.MSex;
using IWM.Services.MStatus;
using IWM.Services.MWard;
using IWM.Services.MWorkerGroup;

namespace IWM.Rpc.worker
{
    public class WorkerController_SingleList : RpcController 
    {
        private readonly IDistrictService DistrictService;
        private readonly INationService NationService;
        private readonly IProvinceService ProvinceService;
        private readonly ISexService SexService;
        private readonly IStatusService StatusService;
        private readonly IWardService WardService;
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly ICurrentContext CurrentContext;
        public WorkerController_SingleList(
            IDistrictService DistrictService,
            INationService NationService,
            IProvinceService ProvinceService,
            ISexService SexService,
            IStatusService StatusService,
            IWardService WardService,
            IWorkerGroupService WorkerGroupService,
            ICurrentContext CurrentContext
        )
        {
            this.DistrictService = DistrictService;
            this.NationService = NationService;
            this.ProvinceService = ProvinceService;
            this.SexService = SexService;
            this.StatusService = StatusService;
            this.WardService = WardService;
            this.WorkerGroupService = WorkerGroupService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerRoute.SingleListDistrict), HttpPost]
        public async Task<List<Worker_DistrictDTO>> SingleListDistrict([FromBody] Worker_DistrictFilterDTO Worker_DistrictFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Id;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = Worker_DistrictFilterDTO.Id;
            DistrictFilter.Code = Worker_DistrictFilterDTO.Code;
            DistrictFilter.Name = Worker_DistrictFilterDTO.Name;
            DistrictFilter.Priority = Worker_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = Worker_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = Worker_DistrictFilterDTO.StatusId;
            DistrictFilter.TrimString();
            DistrictFilter.StatusId = new IdFilter{ Equal = 1 };

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<Worker_DistrictDTO> Worker_DistrictDTOs = Districts
                .Select(x => new Worker_DistrictDTO(x)).ToList();
            return Worker_DistrictDTOs;
        }
        [Route(WorkerRoute.SingleListNation), HttpPost]
        public async Task<List<Worker_NationDTO>> SingleListNation([FromBody] Worker_NationFilterDTO Worker_NationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NationFilter NationFilter = new NationFilter();
            NationFilter.Skip = 0;
            NationFilter.Take = 20;
            NationFilter.OrderBy = NationOrder.Id;
            NationFilter.OrderType = OrderType.ASC;
            NationFilter.Selects = NationSelect.ALL;
            NationFilter.Id = Worker_NationFilterDTO.Id;
            NationFilter.Code = Worker_NationFilterDTO.Code;
            NationFilter.Name = Worker_NationFilterDTO.Name;
            NationFilter.Priority = Worker_NationFilterDTO.Priority;
            NationFilter.StatusId = Worker_NationFilterDTO.StatusId;
            NationFilter.TrimString();
            NationFilter.StatusId = new IdFilter{ Equal = 1 };

            List<Nation> Nations = await NationService.List(NationFilter);
            List<Worker_NationDTO> Worker_NationDTOs = Nations
                .Select(x => new Worker_NationDTO(x)).ToList();
            return Worker_NationDTOs;
        }
        [Route(WorkerRoute.SingleListProvince), HttpPost]
        public async Task<List<Worker_ProvinceDTO>> SingleListProvince([FromBody] Worker_ProvinceFilterDTO Worker_ProvinceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Id;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = Worker_ProvinceFilterDTO.Id;
            ProvinceFilter.Code = Worker_ProvinceFilterDTO.Code;
            ProvinceFilter.Name = Worker_ProvinceFilterDTO.Name;
            ProvinceFilter.Priority = Worker_ProvinceFilterDTO.Priority;
            ProvinceFilter.StatusId = Worker_ProvinceFilterDTO.StatusId;
            ProvinceFilter.TrimString();
            ProvinceFilter.StatusId = new IdFilter{ Equal = 1 };

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<Worker_ProvinceDTO> Worker_ProvinceDTOs = Provinces
                .Select(x => new Worker_ProvinceDTO(x)).ToList();
            return Worker_ProvinceDTOs;
        }
        [Route(WorkerRoute.SingleListSex), HttpPost]
        public async Task<List<Worker_SexDTO>> SingleListSex([FromBody] Worker_SexFilterDTO Worker_SexFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SexFilter SexFilter = new SexFilter();
            SexFilter.Skip = 0;
            SexFilter.Take = int.MaxValue;
            SexFilter.Take = 20;
            SexFilter.OrderBy = SexOrder.Id;
            SexFilter.OrderType = OrderType.ASC;
            SexFilter.Selects = SexSelect.ALL;
            SexFilter.Id = Worker_SexFilterDTO.Id;
            SexFilter.Code = Worker_SexFilterDTO.Code;
            SexFilter.Name = Worker_SexFilterDTO.Name;
            SexFilter.TrimString();

            List<Sex> Sexes = await SexService.List(SexFilter);
            List<Worker_SexDTO> Worker_SexDTOs = Sexes
                .Select(x => new Worker_SexDTO(x)).ToList();
            return Worker_SexDTOs;
        }
        [Route(WorkerRoute.SingleListStatus), HttpPost]
        public async Task<List<Worker_StatusDTO>> SingleListStatus([FromBody] Worker_StatusFilterDTO Worker_StatusFilterDTO)
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
            StatusFilter.Id = Worker_StatusFilterDTO.Id;
            StatusFilter.Code = Worker_StatusFilterDTO.Code;
            StatusFilter.Name = Worker_StatusFilterDTO.Name;
            StatusFilter.Color = Worker_StatusFilterDTO.Color;
            StatusFilter.TrimString();

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Worker_StatusDTO> Worker_StatusDTOs = Statuses
                .Select(x => new Worker_StatusDTO(x)).ToList();
            return Worker_StatusDTOs;
        }
        [Route(WorkerRoute.SingleListWard), HttpPost]
        public async Task<List<Worker_WardDTO>> SingleListWard([FromBody] Worker_WardFilterDTO Worker_WardFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = 0;
            WardFilter.Take = 20;
            WardFilter.OrderBy = WardOrder.Id;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = Worker_WardFilterDTO.Id;
            WardFilter.Code = Worker_WardFilterDTO.Code;
            WardFilter.Name = Worker_WardFilterDTO.Name;
            WardFilter.Priority = Worker_WardFilterDTO.Priority;
            WardFilter.DistrictId = Worker_WardFilterDTO.DistrictId;
            WardFilter.StatusId = Worker_WardFilterDTO.StatusId;
            WardFilter.TrimString();
            WardFilter.StatusId = new IdFilter{ Equal = 1 };

            List<Ward> Wards = await WardService.List(WardFilter);
            List<Worker_WardDTO> Worker_WardDTOs = Wards
                .Select(x => new Worker_WardDTO(x)).ToList();
            return Worker_WardDTOs;
        }
        [Route(WorkerRoute.SingleListWorkerGroup), HttpPost]
        public async Task<List<Worker_WorkerGroupDTO>> SingleListWorkerGroup([FromBody] Worker_WorkerGroupFilterDTO Worker_WorkerGroupFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter();
            WorkerGroupFilter.Skip = 0;
            WorkerGroupFilter.Take = 20;
            WorkerGroupFilter.OrderBy = WorkerGroupOrder.Id;
            WorkerGroupFilter.OrderType = OrderType.ASC;
            WorkerGroupFilter.Selects = WorkerGroupSelect.ALL;
            WorkerGroupFilter.Id = Worker_WorkerGroupFilterDTO.Id;
            WorkerGroupFilter.Code = Worker_WorkerGroupFilterDTO.Code;
            WorkerGroupFilter.Name = Worker_WorkerGroupFilterDTO.Name;
            WorkerGroupFilter.StatusId = Worker_WorkerGroupFilterDTO.StatusId;
            WorkerGroupFilter.TrimString();
            WorkerGroupFilter.StatusId = new IdFilter{ Equal = 1 };

            List<WorkerGroup> WorkerGroups = await WorkerGroupService.List(WorkerGroupFilter);
            List<Worker_WorkerGroupDTO> Worker_WorkerGroupDTOs = WorkerGroups
                .Select(x => new Worker_WorkerGroupDTO(x)).ToList();
            return Worker_WorkerGroupDTOs;
        }
    }
}

