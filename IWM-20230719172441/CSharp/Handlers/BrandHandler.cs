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
using IWM.Services.MBrand;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class BrandHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Brand);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IBrandService Brand = ServiceProvider.GetService<IBrandService>();
                await Sync(Brand, content);
            }
        }

        private async Task Sync(IBrandService BrandService, string json)
        {
            try
            {
                List<Brand> Brands = JsonConvert.DeserializeObject<List<Brand>>(json);
                if (Brands != null && Brands.Count > 0)
                    await BrandService.BulkMerge(Brands);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(BrandHandler));
            }
        }

    }
}
