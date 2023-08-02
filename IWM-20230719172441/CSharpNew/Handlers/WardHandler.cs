using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MWard;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class WardHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IWardService WardService;
        public WardHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IWardService WardService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.WardService = WardService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.WardSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Ward> Wards)
        {
            try
            {
                Initialize(Headers, Wards);
                if (Wards != null && Wards.Count > 0)
                    await WardService.BulkMerge(Wards);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(WardHandler));
            }
        }

    }
}