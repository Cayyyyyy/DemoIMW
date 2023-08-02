using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MNation;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class NationHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly INationService NationService;
        public NationHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, INationService NationService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.NationService = NationService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.NationSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Nation> Nations)
        {
            try
            {
                Initialize(Headers, Nations);
                if (Nations != null && Nations.Count > 0)
                    await NationService.BulkMerge(Nations);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(NationHandler));
            }
        }

    }
}