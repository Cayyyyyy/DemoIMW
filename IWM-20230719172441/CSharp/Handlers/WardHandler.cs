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
using IWM.Services.MWard;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class WardHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Ward);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IWardService Ward = ServiceProvider.GetService<IWardService>();
                await Sync(Ward, content);
            }
        }

        private async Task Sync(IWardService WardService, string json)
        {
            try
            {
                List<Ward> Wards = JsonConvert.DeserializeObject<List<Ward>>(json);
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
