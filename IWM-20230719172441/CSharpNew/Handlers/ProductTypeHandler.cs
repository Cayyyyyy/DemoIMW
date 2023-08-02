using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MProductType;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProductTypeHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IProductTypeService ProductTypeService;
        public ProductTypeHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IProductTypeService ProductTypeService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.ProductTypeService = ProductTypeService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.ProductTypeSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<ProductType> ProductTypes)
        {
            try
            {
                Initialize(Headers, ProductTypes);
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