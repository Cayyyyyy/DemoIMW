using TrueSight;
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
    public class UnitOfMeasureGroupingContentController_List : RpcController
    {
        private readonly IUnitOfMeasureService UnitOfMeasureService;
        private readonly IUnitOfMeasureGroupingService UnitOfMeasureGroupingService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController_List(
            IUnitOfMeasureService UnitOfMeasureService,
            IUnitOfMeasureGroupingService UnitOfMeasureGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.UnitOfMeasureGroupingService = UnitOfMeasureGroupingService;
            this.CurrentContext = CurrentContext;
        }

    }
}