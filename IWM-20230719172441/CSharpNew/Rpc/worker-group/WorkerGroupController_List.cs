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
    public class WorkerGroupController_List : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController_List(
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

    }
}