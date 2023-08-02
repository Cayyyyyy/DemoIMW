using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MDistrict;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class DistrictHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IDistrictService DistrictService;
        public DistrictHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IDistrictService DistrictService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.DistrictService = DistrictService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.DistrictSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<District> Districts)
        {
            try
            {
                Initialize(Headers, Districts);
                if (Districts != null && Districts.Count > 0)
                    await DistrictService.BulkMerge(Districts);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(DistrictHandler));
            }
        }

    }
}