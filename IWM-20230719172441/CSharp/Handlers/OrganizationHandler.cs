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
using IWM.Services.MOrganization;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class OrganizationHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(Organization);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IOrganizationService Organization = ServiceProvider.GetService<IOrganizationService>();
                await Sync(Organization, content);
            }
        }

        private async Task Sync(IOrganizationService OrganizationService, string json)
        {
            try
            {
                List<Organization> Organizations = JsonConvert.DeserializeObject<List<Organization>>(json);
                if (Organizations != null && Organizations.Count > 0)
                    await OrganizationService.BulkMerge(Organizations);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(OrganizationHandler));
            }
        }

    }
}
