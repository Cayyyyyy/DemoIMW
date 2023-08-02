using TrueSight;
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
    public class WorkerController_List : RpcController
    {
        private readonly IDistrictService DistrictService;
        private readonly INationService NationService;
        private readonly IProvinceService ProvinceService;
        private readonly ISexService SexService;
        private readonly IStatusService StatusService;
        private readonly IWardService WardService;
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly ICurrentContext CurrentContext;
        public WorkerController_List(
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

    }
}