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

namespace IWM.Services.MUnitOfMeasure
{
    public interface IUnitOfMeasureService : IServiceScoped
    {
        Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<UnitOfMeasure> Get(long Id);
        Task<List<UnitOfMeasure>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures);
    }

    public class UnitOfMeasureService : BaseService, IUnitOfMeasureService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IUnitOfMeasureValidator UnitOfMeasureValidator;
        public UnitOfMeasureService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IUnitOfMeasureValidator UnitOfMeasureValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureValidator = UnitOfMeasureValidator;
        }

        public async Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter)
        {
            try
            {
                int result = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureService));
            }
        }

        public async Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter)
        {
            try
            {
                List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(UnitOfMeasureFilter);
                return UnitOfMeasures;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureService));
            }
        }

        public async Task<UnitOfMeasure> Get(long Id)
        {
            UnitOfMeasure UnitOfMeasure = await UOW.UnitOfMeasureRepository.Get(Id);
            if (UnitOfMeasure == null)
                return null;
            await UnitOfMeasureValidator.Get(UnitOfMeasure);
            return UnitOfMeasure;
        }
        

        public async Task<List<UnitOfMeasure>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures)
        {
            if (!await UnitOfMeasureValidator.Import(UnitOfMeasures))
                return UnitOfMeasures;
            try
            {
                var Ids = await UOW.UnitOfMeasureRepository.BulkMerge(UnitOfMeasures);
                UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(Ids);
                return UnitOfMeasures;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureService));
            }
        }
        
    }
}
