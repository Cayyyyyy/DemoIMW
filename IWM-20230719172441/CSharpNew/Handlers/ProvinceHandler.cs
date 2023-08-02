using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MProvince;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProvinceHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IProvinceService ProvinceService;
        public ProvinceHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IProvinceService ProvinceService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.ProvinceService = ProvinceService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.ProvinceSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Province> Provinces)
        {
            try
            {
                Initialize(Headers, Provinces);
                if (Provinces != null && Provinces.Count > 0)
                    await ProvinceService.BulkMerge(Provinces);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProvinceHandler));
            }
        }

    }
}