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

namespace IWM.Services.MProvince
{
    public interface IProvinceService :  IServiceScoped
    {
        Task<int> Count(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(ProvinceFilter ProvinceFilter);
        Task<Province> Get(long Id);
        Task<List<Province>> BulkMerge(List<Province> Provinces);
    }

    public class ProvinceService : BaseService, IProvinceService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IProvinceValidator ProvinceValidator;

        public ProvinceService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IProvinceValidator ProvinceValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.ProvinceValidator = ProvinceValidator;
        }

        public async Task<int> Count(ProvinceFilter ProvinceFilter)
        {
            try
            {
                int result = await UOW.ProvinceRepository.Count(ProvinceFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceService));
            }
        }

        public async Task<List<Province>> List(ProvinceFilter ProvinceFilter)
        {
            try
            {
                List<Province> Provinces = await UOW.ProvinceRepository.List(ProvinceFilter);
                return Provinces;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceService));
            }
        }

        public async Task<Province> Get(long Id)
        {
            Province Province = await UOW.ProvinceRepository.Get(Id);
            if (Province == null)
                return null;
            await ProvinceValidator.Get(Province);
            return Province;
        }
        

        public async Task<List<Province>> BulkMerge(List<Province> Provinces)
        {
            if (!await ProvinceValidator.Import(Provinces))
                return Provinces;
            try
            {
                var Ids = await UOW.ProvinceRepository.BulkMerge(Provinces);
                Provinces = await UOW.ProvinceRepository.List(Ids);
                return Provinces;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceService));
            }
        }     
        
    }
}
