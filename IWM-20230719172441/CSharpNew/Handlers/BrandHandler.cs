using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MBrand;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class BrandHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IBrandService BrandService;
        public BrandHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IBrandService BrandService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.BrandService = BrandService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.BrandSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Brand> Brands)
        {
            try
            {
                Initialize(Headers, Brands);
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