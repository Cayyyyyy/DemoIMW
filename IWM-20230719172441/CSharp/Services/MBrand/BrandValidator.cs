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

namespace IWM.Services.MBrand
{
    public interface IBrandValidator : IServiceScoped
    {
        Task Get(Brand Brand);
        Task<bool> Import(List<Brand> Brands);
    }

    public class BrandValidator : IBrandValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private BrandMessage BrandMessage;

        public BrandValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.BrandMessage = new BrandMessage();
        }

        public async Task Get(Brand Brand)
        {
        }


        public async Task<bool> Import(List<Brand> Brands)
        {
            return true;
        }
        
    }
}
