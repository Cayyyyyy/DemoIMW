using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MSex;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class SexHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly ISexService SexService;
        public SexHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, ISexService SexService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.SexService = SexService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.SexSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Sex> Sexes)
        {
            try
            {
                Initialize(Headers, Sexes);
                if (Sexes != null && Sexes.Count > 0)
                    await SexService.BulkMerge(Sexes);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(SexHandler));
            }
        }

    }
}