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

namespace IWM.Services.MTaxType
{
    public interface ITaxTypeValidator : IServiceScoped
    {
        Task Get(TaxType TaxType);
        Task<bool> Import(List<TaxType> TaxTypes);
    }

    public class TaxTypeValidator : ITaxTypeValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private TaxTypeMessage TaxTypeMessage;

        public TaxTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.TaxTypeMessage = new TaxTypeMessage();
        }

        public async Task Get(TaxType TaxType)
        {
        }


        public async Task<bool> Import(List<TaxType> TaxTypes)
        {
            return true;
        }
        
    }
}
