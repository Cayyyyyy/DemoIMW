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

namespace IWM.Services.MAppUser
{
    public interface IAppUserService :  IServiceScoped
    {
        Task<int> Count(AppUserFilter AppUserFilter);
        Task<List<AppUser>> List(AppUserFilter AppUserFilter);
        Task<AppUser> Get(long Id);
        Task<List<AppUser>> BulkMerge(List<AppUser> AppUsers);
    }

    public class AppUserService : BaseService, IAppUserService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IAppUserValidator AppUserValidator;

        public AppUserService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IAppUserValidator AppUserValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.AppUserValidator = AppUserValidator;
        }

        public async Task<int> Count(AppUserFilter AppUserFilter)
        {
            try
            {
                int result = await UOW.AppUserRepository.Count(AppUserFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(AppUserService));
            }
        }

        public async Task<List<AppUser>> List(AppUserFilter AppUserFilter)
        {
            try
            {
                List<AppUser> AppUsers = await UOW.AppUserRepository.List(AppUserFilter);
                return AppUsers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(AppUserService));
            }
        }

        public async Task<AppUser> Get(long Id)
        {
            AppUser AppUser = await UOW.AppUserRepository.Get(Id);
            if (AppUser == null)
                return null;
            await AppUserValidator.Get(AppUser);
            return AppUser;
        }
        

        public async Task<List<AppUser>> BulkMerge(List<AppUser> AppUsers)
        {
            if (!await AppUserValidator.Import(AppUsers))
                return AppUsers;
            try
            {
                var Ids = await UOW.AppUserRepository.BulkMerge(AppUsers);
                AppUsers = await UOW.AppUserRepository.List(Ids);
                return AppUsers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(AppUserService));
            }
        }     
        
    }
}
