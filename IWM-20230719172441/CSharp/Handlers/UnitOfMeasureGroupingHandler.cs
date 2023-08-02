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
using IWM.Services.MUnitOfMeasureGrouping;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class UnitOfMeasureGroupingHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(UnitOfMeasureGrouping);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IUnitOfMeasureGroupingService UnitOfMeasureGrouping = ServiceProvider.GetService<IUnitOfMeasureGroupingService>();
                await Sync(UnitOfMeasureGrouping, content);
            }
        }

        private async Task Sync(IUnitOfMeasureGroupingService UnitOfMeasureGroupingService, string json)
        {
            try
            {
                List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = JsonConvert.DeserializeObject<List<UnitOfMeasureGrouping>>(json);
                if (UnitOfMeasureGroupings != null && UnitOfMeasureGroupings.Count > 0)
                    await UnitOfMeasureGroupingService.BulkMerge(UnitOfMeasureGroupings);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(UnitOfMeasureGroupingHandler));
            }
        }

    }
}
