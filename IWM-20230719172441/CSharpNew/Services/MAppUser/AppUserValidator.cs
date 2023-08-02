using TrueSight;
using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System.Text.RegularExpressions;

namespace IWM.Services.MAppUser
{
    public interface IAppUserValidator : IServiceScoped
    {
        Task Get(AppUser AppUser);
        Task<bool> Import(List<AppUser> AppUsers);
    }

    public class AppUserValidator : IAppUserValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private AppUserMessage AppUserMessage;

        public AppUserValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(AppUserValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.AppUserMessage = new AppUserMessage();
        }

        public async Task Get(AppUser AppUser)
        {
        }


        public async Task<bool> Import(List<AppUser> AppUsers)
        {
            return true;
        }
        
    }
}
