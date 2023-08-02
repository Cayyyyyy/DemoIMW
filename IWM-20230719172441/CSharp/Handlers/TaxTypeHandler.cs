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
using IWM.Services.MTaxType;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class TaxTypeHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(TaxType);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                ITaxTypeService TaxType = ServiceProvider.GetService<ITaxTypeService>();
                await Sync(TaxType, content);
            }
        }

        private async Task Sync(ITaxTypeService TaxTypeService, string json)
        {
            try
            {
                List<TaxType> TaxTypes = JsonConvert.DeserializeObject<List<TaxType>>(json);
                if (TaxTypes != null && TaxTypes.Count > 0)
                    await TaxTypeService.BulkMerge(TaxTypes);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(TaxTypeHandler));
            }
        }

    }
}
