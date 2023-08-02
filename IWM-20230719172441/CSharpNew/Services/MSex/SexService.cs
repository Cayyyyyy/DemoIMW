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

namespace IWM.Services.MSex
{
    public interface ISexService : IServiceScoped
    {
        Task<int> Count(SexFilter SexFilter);
        Task<List<Sex>> List(SexFilter SexFilter);
        Task<List<Sex>> BulkMerge(List<Sex> Sexes);
    }

    public class SexService : BaseService, ISexService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        public SexService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
        }

        public async Task<int> Count(SexFilter SexFilter)
        {
            try
            {
                int result = await UOW.SexRepository.Count(SexFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(SexService));
            }
        }

        public async Task<List<Sex>> List(SexFilter SexFilter)
        {
            try
            {
                List<Sex> Sexes = await UOW.SexRepository.List(SexFilter);
                return Sexes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(SexService));
            }
        }


        public async Task<List<Sex>> BulkMerge(List<Sex> Sexes)
        {
            try
            {
                var Ids = await UOW.SexRepository.BulkMerge(Sexes);
                return Sexes;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(SexService));
            }
        }
        
    }
}
