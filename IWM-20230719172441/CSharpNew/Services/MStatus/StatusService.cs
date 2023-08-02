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

namespace IWM.Services.MStatus
{
    public interface IStatusService : IServiceScoped
    {
        Task<int> Count(StatusFilter StatusFilter);
        Task<List<Status>> List(StatusFilter StatusFilter);
        Task<List<Status>> BulkMerge(List<Status> Statuses);
    }

    public class StatusService : BaseService, IStatusService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        public StatusService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
        }

        public async Task<int> Count(StatusFilter StatusFilter)
        {
            try
            {
                int result = await UOW.StatusRepository.Count(StatusFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(StatusService));
            }
        }

        public async Task<List<Status>> List(StatusFilter StatusFilter)
        {
            try
            {
                List<Status> Statuses = await UOW.StatusRepository.List(StatusFilter);
                return Statuses;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(StatusService));
            }
        }


        public async Task<List<Status>> BulkMerge(List<Status> Statuses)
        {
            try
            {
                var Ids = await UOW.StatusRepository.BulkMerge(Statuses);
                return Statuses;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(StatusService));
            }
        }
        
    }
}
