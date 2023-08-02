using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MUnitOfMeasure;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class UnitOfMeasureHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IUnitOfMeasureService UnitOfMeasureService;
        public UnitOfMeasureHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IUnitOfMeasureService UnitOfMeasureService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.UnitOfMeasureService = UnitOfMeasureService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.UnitOfMeasureSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<UnitOfMeasure> UnitOfMeasures)
        {
            try
            {
                Initialize(Headers, UnitOfMeasures);
                if (UnitOfMeasures != null && UnitOfMeasures.Count > 0)
                    await UnitOfMeasureService.BulkMerge(UnitOfMeasures);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(UnitOfMeasureHandler));
            }
        }

    }
}