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

namespace IWM.Services.MProductType
{
    public interface IProductTypeService :  IServiceScoped
    {
        Task<int> Count(ProductTypeFilter ProductTypeFilter);
        Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter);
        Task<ProductType> Get(long Id);
        Task<List<ProductType>> BulkMerge(List<ProductType> ProductTypes);
    }

    public class ProductTypeService : BaseService, IProductTypeService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IProductTypeValidator ProductTypeValidator;

        public ProductTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IProductTypeValidator ProductTypeValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.ProductTypeValidator = ProductTypeValidator;
        }

        public async Task<int> Count(ProductTypeFilter ProductTypeFilter)
        {
            try
            {
                int result = await UOW.ProductTypeRepository.Count(ProductTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductTypeService));
            }
        }

        public async Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter)
        {
            try
            {
                List<ProductType> ProductTypes = await UOW.ProductTypeRepository.List(ProductTypeFilter);
                return ProductTypes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductTypeService));
            }
        }

        public async Task<ProductType> Get(long Id)
        {
            ProductType ProductType = await UOW.ProductTypeRepository.Get(Id);
            if (ProductType == null)
                return null;
            await ProductTypeValidator.Get(ProductType);
            return ProductType;
        }
        

        public async Task<List<ProductType>> BulkMerge(List<ProductType> ProductTypes)
        {
            if (!await ProductTypeValidator.Import(ProductTypes))
                return ProductTypes;
            try
            {
                var Ids = await UOW.ProductTypeRepository.BulkMerge(ProductTypes);
                ProductTypes = await UOW.ProductTypeRepository.List(Ids);
                return ProductTypes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProductTypeService));
            }
        }     
        
    }
}
