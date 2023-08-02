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

namespace IWM.Services.MTaxType
{
    public interface ITaxTypeService :  IServiceScoped
    {
        Task<int> Count(TaxTypeFilter TaxTypeFilter);
        Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter);
        Task<TaxType> Get(long Id);
        Task<List<TaxType>> BulkMerge(List<TaxType> TaxTypes);
    }

    public class TaxTypeService : BaseService, ITaxTypeService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly ITaxTypeValidator TaxTypeValidator;

        public TaxTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ITaxTypeValidator TaxTypeValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.TaxTypeValidator = TaxTypeValidator;
        }

        public async Task<int> Count(TaxTypeFilter TaxTypeFilter)
        {
            try
            {
                int result = await UOW.TaxTypeRepository.Count(TaxTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(TaxTypeService));
            }
        }

        public async Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter)
        {
            try
            {
                List<TaxType> TaxTypes = await UOW.TaxTypeRepository.List(TaxTypeFilter);
                return TaxTypes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(TaxTypeService));
            }
        }

        public async Task<TaxType> Get(long Id)
        {
            TaxType TaxType = await UOW.TaxTypeRepository.Get(Id);
            if (TaxType == null)
                return null;
            await TaxTypeValidator.Get(TaxType);
            return TaxType;
        }
        

        public async Task<List<TaxType>> BulkMerge(List<TaxType> TaxTypes)
        {
            if (!await TaxTypeValidator.Import(TaxTypes))
                return TaxTypes;
            try
            {
                var Ids = await UOW.TaxTypeRepository.BulkMerge(TaxTypes);
                TaxTypes = await UOW.TaxTypeRepository.List(Ids);
                return TaxTypes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(TaxTypeService));
            }
        }     
        
    }
}
