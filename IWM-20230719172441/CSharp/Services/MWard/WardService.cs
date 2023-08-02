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

namespace IWM.Services.MWard
{
    public interface IWardService :  IServiceScoped
    {
        Task<int> Count(WardFilter WardFilter);
        Task<List<Ward>> List(WardFilter WardFilter);
        Task<Ward> Get(long Id);
        Task<List<Ward>> BulkMerge(List<Ward> Wards);
    }

    public class WardService : BaseService, IWardService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IWardValidator WardValidator;

        public WardService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IWardValidator WardValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.WardValidator = WardValidator;
        }

        public async Task<int> Count(WardFilter WardFilter)
        {
            try
            {
                int result = await UOW.WardRepository.Count(WardFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WardService));
            }
        }

        public async Task<List<Ward>> List(WardFilter WardFilter)
        {
            try
            {
                List<Ward> Wards = await UOW.WardRepository.List(WardFilter);
                return Wards;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WardService));
            }
        }

        public async Task<Ward> Get(long Id)
        {
            Ward Ward = await UOW.WardRepository.Get(Id);
            if (Ward == null)
                return null;
            await WardValidator.Get(Ward);
            return Ward;
        }
        

        public async Task<List<Ward>> BulkMerge(List<Ward> Wards)
        {
            if (!await WardValidator.Import(Wards))
                return Wards;
            try
            {
                var Ids = await UOW.WardRepository.BulkMerge(Wards);
                Wards = await UOW.WardRepository.List(Ids);
                return Wards;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WardService));
            }
        }     
        
    }
}
