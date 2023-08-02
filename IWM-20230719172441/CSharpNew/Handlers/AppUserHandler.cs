using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MAppUser;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class AppUserHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IAppUserService AppUserService;
        public AppUserHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IAppUserService AppUserService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.AppUserService = AppUserService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.AppUserSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<AppUser> AppUsers)
        {
            try
            {
                Initialize(Headers, AppUsers);
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