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
using IWM.Services.MUnitOfMeasure;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class UnitOfMeasureHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(UnitOfMeasure);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IUnitOfMeasureService UnitOfMeasure = ServiceProvider.GetService<IUnitOfMeasureService>();
                await Sync(UnitOfMeasure, content);
            }
        }

        private async Task Sync(IUnitOfMeasureService UnitOfMeasureService, string json)
        {
            try
            {
                List<UnitOfMeasure> UnitOfMeasures = JsonConvert.DeserializeObject<List<UnitOfMeasure>>(json);
                if (UnitOfMeasures != null && UnitOfMeasures.Count > 0)
                    await UnitOfMeasureService.BulkMerge(UnitOfMeasures);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(UnitOfMeasureHandler));
            }
        }

    }
}
