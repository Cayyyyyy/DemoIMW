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

namespace IWM.Services.MProvince
{
    public interface IProvinceValidator : IServiceScoped
    {
        Task Get(Province Province);
        Task<bool> Import(List<Province> Provinces);
    }

    public class ProvinceValidator : IProvinceValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private ProvinceMessage ProvinceMessage;

        public ProvinceValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(ProvinceValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.ProvinceMessage = new ProvinceMessage();
        }

        public async Task Get(Province Province)
        {
        }


        public async Task<bool> Import(List<Province> Provinces)
        {
            return true;
        }
        
    }
}
