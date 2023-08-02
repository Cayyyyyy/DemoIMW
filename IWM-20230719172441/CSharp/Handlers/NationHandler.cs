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
using IWM.Services.MNation;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class NationHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Nation);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                INationService Nation = ServiceProvider.GetService<INationService>();
                await Sync(Nation, content);
            }
        }

        private async Task Sync(INationService NationService, string json)
        {
            try
            {
                List<Nation> Nations = JsonConvert.DeserializeObject<List<Nation>>(json);
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
