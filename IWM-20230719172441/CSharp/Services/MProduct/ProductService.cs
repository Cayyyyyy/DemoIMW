using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using IWM.Repositories;
using IWM.Entities;
using IWM.Enums;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MProduct
{
    public interface IProductService :  IServiceScoped
    {
        Task<int> Count(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<Product> Get(long Id);
        Task<List<Product>> BulkMerge(List<Product> Products);
    }

    public class ProductService : BaseService, IProductService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IProductValidator ProductValidator;

        public ProductService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IProductValidator ProductValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.ProductValidator = ProductValidator;
        }

        public async Task<int> Count(ProductFilter ProductFilter)
        {
            try
            {
                int result = await UOW.ProductRepository.Count(ProductFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductService));
            }
        }

        public async Task<List<Product>> List(ProductFilter ProductFilter)
        {
            try
            {
                List<Product> Products = await UOW.ProductRepository.List(ProductFilter);
                return Products;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductService));
            }
        }

        public async Task<Product> Get(long Id)
        {
            Product Product = await UOW.ProductRepository.Get(Id);
            if (Product == null)
                return null;
            await ProductValidator.Get(Product);
            return Product;
        }
        

        public async Task<List<Product>> BulkMerge(List<Product> Products)
        {
            if (!await ProductValidator.Import(Products))
                return Products;
            try
            {
                var Ids = await UOW.ProductRepository.BulkMerge(Products);
                Products = await UOW.ProductRepository.List(Ids);
                return Products;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductService));
            }
        }     
        
    }
}
