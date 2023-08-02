using TrueSight.Common;
using TrueSight.RQHistory.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IWM.Common;
using IWM.Entities;
using IWM.Services.MStatus;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController_List : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController_List(
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

    }
}