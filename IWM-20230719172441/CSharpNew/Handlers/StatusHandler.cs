using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MStatus;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class StatusHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IStatusService StatusService;
        public StatusHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IStatusService StatusService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.StatusService = StatusService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.StatusSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Status> Statuses)
        {
            try
            {
                Initialize(Headers, Statuses);
                if (Statuses != null && Statuses.Count > 0)
                    await StatusService.BulkMerge(Statuses);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StatusHandler));
            }
        }

    }
}