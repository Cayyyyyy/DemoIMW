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

namespace IWM.Services.MDistrict
{
    public interface IDistrictService :  IServiceScoped
    {
        Task<int> Count(DistrictFilter DistrictFilter);
        Task<List<District>> List(DistrictFilter DistrictFilter);
        Task<District> Get(long Id);
        Task<List<District>> BulkMerge(List<District> Districts);
    }

    public class DistrictService : BaseService, IDistrictService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IDistrictValidator DistrictValidator;

        public DistrictService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IDistrictValidator DistrictValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.DistrictValidator = DistrictValidator;
        }

        public async Task<int> Count(DistrictFilter DistrictFilter)
        {
            try
            {
                int result = await UOW.DistrictRepository.Count(DistrictFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(DistrictService));
            }
        }

        public async Task<List<District>> List(DistrictFilter DistrictFilter)
        {
            try
            {
                List<District> Districts = await UOW.DistrictRepository.List(DistrictFilter);
                return Districts;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(DistrictService));
            }
        }

        public async Task<District> Get(long Id)
        {
            District District = await UOW.DistrictRepository.Get(Id);
            if (District == null)
                return null;
            await DistrictValidator.Get(District);
            return District;
        }
        

        public async Task<List<District>> BulkMerge(List<District> Districts)
        {
            if (!await DistrictValidator.Import(Districts))
                return Districts;
            try
            {
                var Ids = await UOW.DistrictRepository.BulkMerge(Districts);
                Districts = await UOW.DistrictRepository.List(Ids);
                return Districts;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(DistrictService));
            }
        }     
        
    }
}
