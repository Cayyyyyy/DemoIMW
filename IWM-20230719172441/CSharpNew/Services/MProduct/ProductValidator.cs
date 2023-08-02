using TrueSight;
using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System.Text.RegularExpressions;

namespace IWM.Services.MProduct
{
    public interface IProductValidator : IServiceScoped
    {
        Task Get(Product Product);
        Task<bool> Import(List<Product> Products);
    }

    public class ProductValidator : IProductValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private ProductMessage ProductMessage;

        public ProductValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(ProductValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.ProductMessage = new ProductMessage();
        }

        public async Task Get(Product Product)
        {
        }


        public async Task<bool> Import(List<Product> Products)
        {
            return true;
        }
        
    }
}
