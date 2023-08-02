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
using IWM.Services.MProduct;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProductHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Product);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IProductService Product = ServiceProvider.GetService<IProductService>();
                await Sync(Product, content);
            }
        }

        private async Task Sync(IProductService ProductService, string json)
        {
            try
            {
                List<Product> Products = JsonConvert.DeserializeObject<List<Product>>(json);
                if (Products != null && Products.Count > 0)
                    await ProductService.BulkMerge(Products);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProductHandler));
            }
        }

    }
}
