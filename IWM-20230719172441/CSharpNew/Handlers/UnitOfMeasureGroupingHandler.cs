using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MUnitOfMeasureGrouping;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class UnitOfMeasureGroupingHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IUnitOfMeasureGroupingService UnitOfMeasureGroupingService;
        public UnitOfMeasureGroupingHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IUnitOfMeasureGroupingService UnitOfMeasureGroupingService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.UnitOfMeasureGroupingService = UnitOfMeasureGroupingService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.UnitOfMeasureGroupingSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            try
            {
                Initialize(Headers, UnitOfMeasureGroupings);
                if (UnitOfMeasureGroupings != null && UnitOfMeasureGroupings.Count > 0)
                    await UnitOfMeasureGroupingService.BulkMerge(UnitOfMeasureGroupings);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(UnitOfMeasureGroupingHandler));
            }
        }

    }
}