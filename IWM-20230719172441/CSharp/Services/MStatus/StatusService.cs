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

namespace IWM.Services.MStatus
{
    public interface IStatusService :  IEnumServiceScoped
    {
        Task<int> Count(StatusFilter StatusFilter);
        Task<List<Status>> List(StatusFilter StatusFilter);
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

        public async Task Initialize()
        {
            try
            {
                List<Status> Statuses = await UOW.StatusRepository.List(new StatusFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StatusSelect.ALL
                });
                foreach (var item in StatusEnum.StatusEnumList)
                {
                    var Status = Statuses.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (Status == null)
                    {
                        Status = new Status();
                        Statuses.Add(Status);
                    }
                    Status.Id = item.Id;
                    Status.Code = item.Code;
                    Status.Name = item.Name;
                    Status.Color = item.Color;
                }
                await UOW.StatusRepository.BulkMerge(Statuses);
                RabbitManager.PublishList(Statuses, MessageRoutingKey.StatusSync);
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(StatusService));
            }
        }
    }
}
