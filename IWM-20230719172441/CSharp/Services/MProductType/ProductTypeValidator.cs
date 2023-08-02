using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM;
using IWM.Common;
using IWM.Enums;
using IWM.Entities;
using IWM.Repositories;
using System.Text.RegularExpressions;

namespace IWM.Services.MProductType
{
    public interface IProductTypeValidator : IServiceScoped
    {
        Task Get(ProductType ProductType);
        Task<bool> Import(List<ProductType> ProductTypes);
    }

    public class ProductTypeValidator : IProductTypeValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private ProductTypeMessage ProductTypeMessage;

        public ProductTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.ProductTypeMessage = new ProductTypeMessage();
        }

        public async Task Get(ProductType ProductType)
        {
        }


        public async Task<bool> Import(List<ProductType> ProductTypes)
        {
            return true;
        }
        
    }
}
