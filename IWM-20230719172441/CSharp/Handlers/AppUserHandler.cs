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
using IWM.Services.MAppUser;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class AppUserHandler : Handler
    {
        private string SyncKey => Name + MessageRoutingKey.BaseSyncData;
        public override string Name => nameof(AppUser);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {   
            if (routingKey == SyncKey)
            {
                IAppUserService AppUser = ServiceProvider.GetService<IAppUserService>();
                await Sync(AppUser, content);
            }
        }

        private async Task Sync(IAppUserService AppUserService, string json)
        {
            try
            {
                List<AppUser> AppUsers = JsonConvert.DeserializeObject<List<AppUser>>(json);
                if (AppUsers != null && AppUsers.Count > 0)
                    await AppUserService.BulkMerge(AppUsers);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(AppUserHandler));
            }
        }

    }
}
