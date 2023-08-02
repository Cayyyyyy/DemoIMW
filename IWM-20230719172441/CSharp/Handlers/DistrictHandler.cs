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
using IWM.Services.MDistrict;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class DistrictHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(District);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IDistrictService District = ServiceProvider.GetService<IDistrictService>();
                await Sync(District, content);
            }
        }

        private async Task Sync(IDistrictService DistrictService, string json)
        {
            try
            {
                List<District> Districts = JsonConvert.DeserializeObject<List<District>>(json);
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
