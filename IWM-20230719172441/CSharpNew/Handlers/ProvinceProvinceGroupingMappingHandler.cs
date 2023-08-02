using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MProvinceProvinceGroupingMapping;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProvinceProvinceGroupingMappingHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IProvinceProvinceGroupingMappingService ProvinceProvinceGroupingMappingService;
        public ProvinceProvinceGroupingMappingHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IProvinceProvinceGroupingMappingService ProvinceProvinceGroupingMappingService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.ProvinceProvinceGroupingMappingService = ProvinceProvinceGroupingMappingService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.ProvinceProvinceGroupingMappingSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<ProvinceProvinceGroupingMapping> ProvinceProvinceGroupingMappings)
        {
            try
            {
                Initialize(Headers, ProvinceProvinceGroupingMappings);
                if (ProvinceProvinceGroupingMappings != null && ProvinceProvinceGroupingMappings.Count > 0)
                    await ProvinceProvinceGroupingMappingService.BulkMerge(ProvinceProvinceGroupingMappings);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProvinceProvinceGroupingMappingHandler));
            }
        }

    }
}