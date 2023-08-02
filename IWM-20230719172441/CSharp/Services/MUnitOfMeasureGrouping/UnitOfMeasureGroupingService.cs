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

namespace IWM.Services.MUnitOfMeasureGrouping
{
    public interface IUnitOfMeasureGroupingService :  IServiceScoped
    {
        Task<int> Count(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<UnitOfMeasureGrouping> Get(long Id);
        Task<List<UnitOfMeasureGrouping>> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
    }

    public class UnitOfMeasureGroupingService : BaseService, IUnitOfMeasureGroupingService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IUnitOfMeasureGroupingValidator UnitOfMeasureGroupingValidator;

        public UnitOfMeasureGroupingService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IUnitOfMeasureGroupingValidator UnitOfMeasureGroupingValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureGroupingValidator = UnitOfMeasureGroupingValidator;
        }

        public async Task<int> Count(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter)
        {
            try
            {
                int result = await UOW.UnitOfMeasureGroupingRepository.Count(UnitOfMeasureGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingService));
            }
        }

        public async Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter)
        {
            try
            {
                List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await UOW.UnitOfMeasureGroupingRepository.List(UnitOfMeasureGroupingFilter);
                return UnitOfMeasureGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingService));
            }
        }

        public async Task<UnitOfMeasureGrouping> Get(long Id)
        {
            UnitOfMeasureGrouping UnitOfMeasureGrouping = await UOW.UnitOfMeasureGroupingRepository.Get(Id);
            if (UnitOfMeasureGrouping == null)
                return null;
            await UnitOfMeasureGroupingValidator.Get(UnitOfMeasureGrouping);
            return UnitOfMeasureGrouping;
        }
        

        public async Task<List<UnitOfMeasureGrouping>> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            if (!await UnitOfMeasureGroupingValidator.Import(UnitOfMeasureGroupings))
                return UnitOfMeasureGroupings;
            try
            {
                var Ids = await UOW.UnitOfMeasureGroupingRepository.BulkMerge(UnitOfMeasureGroupings);
                UnitOfMeasureGroupings = await UOW.UnitOfMeasureGroupingRepository.List(Ids);
                return UnitOfMeasureGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingService));
            }
        }     
        
    }
}
