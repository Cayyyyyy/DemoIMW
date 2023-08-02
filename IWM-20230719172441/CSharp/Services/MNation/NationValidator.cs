using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM;
using IWM.Common;
using IWM.Enums;
using IWM.Entities;
using IWM.Repositories;
using System.Text.RegularExpressions;

namespace IWM.Services.MNation
{
    public interface INationValidator : IServiceScoped
    {
        Task Get(Nation Nation);
        Task<bool> Import(List<Nation> Nations);
    }

    public class NationValidator : INationValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private NationMessage NationMessage;

        public NationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.NationMessage = new NationMessage();
        }

        public async Task Get(Nation Nation)
        {
        }


        public async Task<bool> Import(List<Nation> Nations)
        {
            return true;
        }
        
    }
}
