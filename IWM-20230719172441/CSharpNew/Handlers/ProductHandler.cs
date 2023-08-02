using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MProduct;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class ProductHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly IProductService ProductService;
        public ProductHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, IProductService ProductService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.ProductService = ProductService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.ProductSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<Product> Products)
        {
            try
            {
                Initialize(Headers, Products);
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