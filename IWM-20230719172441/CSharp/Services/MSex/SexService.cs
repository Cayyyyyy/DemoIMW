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

namespace IWM.Services.MSex
{
    public interface ISexService :  IEnumServiceScoped
    {
        Task<int> Count(SexFilter SexFilter);
        Task<List<Sex>> List(SexFilter SexFilter);
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

        public async Task Initialize()
        {
            try
            {
                List<Sex> Sexes = await UOW.SexRepository.List(new SexFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = SexSelect.ALL
                });
                foreach (var item in SexEnum.SexEnumList)
                {
                    var Sex = Sexes.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (Sex == null)
                    {
                        Sex = new Sex();
                        Sexes.Add(Sex);
                    }
                    Sex.Id = item.Id;
                    Sex.Code = item.Code;
                    Sex.Name = item.Name;
                }
                await UOW.SexRepository.BulkMerge(Sexes);
                RabbitManager.PublishList(Sexes, MessageRoutingKey.SexSync);
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(SexService));
            }
        }
    }
}
