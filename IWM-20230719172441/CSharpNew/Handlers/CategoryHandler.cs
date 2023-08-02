using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MCategory;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class CategoryHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly ICategoryService CategoryService;
        public CategoryHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, ICategoryService CategoryService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.CategoryService = CategoryService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.CategorySync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Category> Categories)
        {
            try
            {
                Initialize(Headers, Categories);
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