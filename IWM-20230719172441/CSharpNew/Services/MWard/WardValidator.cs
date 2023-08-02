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

namespace IWM.Services.MWard
{
    public interface IWardValidator : IServiceScoped
    {
        Task Get(Ward Ward);
        Task<bool> Import(List<Ward> Wards);
    }

    public class WardValidator : IWardValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private WardMessage WardMessage;

        public WardValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(WardValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.WardMessage = new WardMessage();
        }

        public async Task Get(Ward Ward)
        {
        }


        public async Task<bool> Import(List<Ward> Wards)
        {
            return true;
        }
        
    }
}
