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

namespace IWM.Services.MNation
{
    public interface INationService :  IServiceScoped
    {
        Task<int> Count(NationFilter NationFilter);
        Task<List<Nation>> List(NationFilter NationFilter);
        Task<Nation> Get(long Id);
        Task<List<Nation>> BulkMerge(List<Nation> Nations);
    }

    public class NationService : BaseService, INationService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly INationValidator NationValidator;

        public NationService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            INationValidator NationValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.NationValidator = NationValidator;
        }

        public async Task<int> Count(NationFilter NationFilter)
        {
            try
            {
                int result = await UOW.NationRepository.Count(NationFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(NationService));
            }
        }

        public async Task<List<Nation>> List(NationFilter NationFilter)
        {
            try
            {
                List<Nation> Nations = await UOW.NationRepository.List(NationFilter);
                return Nations;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(NationService));
            }
        }

        public async Task<Nation> Get(long Id)
        {
            Nation Nation = await UOW.NationRepository.Get(Id);
            if (Nation == null)
                return null;
            await NationValidator.Get(Nation);
            return Nation;
        }
        

        public async Task<List<Nation>> BulkMerge(List<Nation> Nations)
        {
            if (!await NationValidator.Import(Nations))
                return Nations;
            try
            {
                var Ids = await UOW.NationRepository.BulkMerge(Nations);
                Nations = await UOW.NationRepository.List(Ids);
                return Nations;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(NationService));
            }
        }     
        
    }
}
