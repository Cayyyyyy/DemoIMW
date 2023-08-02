using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Enums;
using IWM.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using IWM.Services.MProvinceProvinceGroupingMapping;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProvinceProvinceGroupingMappingHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(ProvinceProvinceGroupingMapping);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IProvinceProvinceGroupingMappingService ProvinceProvinceGroupingMapping = ServiceProvider.GetService<IProvinceProvinceGroupingMappingService>();
                await Sync(ProvinceProvinceGroupingMapping, content);
            }
        }

        private async Task Sync(IProvinceProvinceGroupingMappingService ProvinceProvinceGroupingMappingService, string json)
        {
            try
            {
                List<ProvinceProvinceGroupingMapping> ProvinceProvinceGroupingMappings = JsonConvert.DeserializeObject<List<ProvinceProvinceGroupingMapping>>(json);
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
