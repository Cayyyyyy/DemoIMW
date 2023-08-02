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

namespace IWM.Services.MDistrict
{
    public interface IDistrictValidator : IServiceScoped
    {
        Task Get(District District);
        Task<bool> Import(List<District> Districts);
    }

    public class DistrictValidator : IDistrictValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private DistrictMessage DistrictMessage;

        public DistrictValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(DistrictValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.DistrictMessage = new DistrictMessage();
        }

        public async Task Get(District District)
        {
        }


        public async Task<bool> Import(List<District> Districts)
        {
            return true;
        }
        
    }
}
