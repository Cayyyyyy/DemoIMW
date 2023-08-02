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
using IWM.Services.MProvince;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProvinceHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Province);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IProvinceService Province = ServiceProvider.GetService<IProvinceService>();
                await Sync(Province, content);
            }
        }

        private async Task Sync(IProvinceService ProvinceService, string json)
        {
            try
            {
                List<Province> Provinces = JsonConvert.DeserializeObject<List<Province>>(json);
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
