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
using IWM.Services.MCategory;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class CategoryHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Category);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                ICategoryService Category = ServiceProvider.GetService<ICategoryService>();
                await Sync(Category, content);
            }
        }

        private async Task Sync(ICategoryService CategoryService, string json)
        {
            try
            {
                List<Category> Categories = JsonConvert.DeserializeObject<List<Category>>(json);
                if (Categories != null && Categories.Count > 0)
                    await CategoryService.BulkMerge(Categories);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(CategoryHandler));
            }
        }

    }
}
