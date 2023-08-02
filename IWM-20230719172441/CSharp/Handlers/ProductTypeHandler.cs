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
using IWM.Services.MProductType;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProductTypeHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(ProductType);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IProductTypeService ProductType = ServiceProvider.GetService<IProductTypeService>();
                await Sync(ProductType, content);
            }
        }

        private async Task Sync(IProductTypeService ProductTypeService, string json)
        {
            try
            {
                List<ProductType> ProductTypes = JsonConvert.DeserializeObject<List<ProductType>>(json);
                if (ProductTypes != null && ProductTypes.Count > 0)
                    await ProductTypeService.BulkMerge(ProductTypes);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProductTypeHandler));
            }
        }

    }
}
