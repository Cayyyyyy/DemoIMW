using TrueSight;
using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using IWM.Repositories;
using IWM.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MBrand
{
    public interface IBrandService : IServiceScoped
    {
        Task<int> Count(BrandFilter BrandFilter);
        Task<List<Brand>> List(BrandFilter BrandFilter);
        Task<Brand> Get(long Id);
        Task<List<Brand>> BulkMerge(List<Brand> Brands);
    }

    public class BrandService : BaseService, IBrandService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IBrandValidator BrandValidator;
        public BrandService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IBrandValidator BrandValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.BrandValidator = BrandValidator;
        }

        public async Task<int> Count(BrandFilter BrandFilter)
        {
            try
            {
                int result = await UOW.BrandRepository.Count(BrandFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(BrandService));
            }
        }

        public async Task<List<Brand>> List(BrandFilter BrandFilter)
        {
            try
            {
                List<Brand> Brands = await UOW.BrandRepository.List(BrandFilter);
                return Brands;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(BrandService));
            }
        }

        public async Task<Brand> Get(long Id)
        {
            Brand Brand = await UOW.BrandRepository.Get(Id);
            if (Brand == null)
                return null;
            await BrandValidator.Get(Brand);
            return Brand;
        }
        

        public async Task<List<Brand>> BulkMerge(List<Brand> Brands)
        {
            if (!await BrandValidator.Import(Brands))
                return Brands;
            try
            {
                var Ids = await UOW.BrandRepository.BulkMerge(Brands);
                Brands = await UOW.BrandRepository.List(Ids);
                return Brands;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(BrandService));
            }
        }
        
    }
}
