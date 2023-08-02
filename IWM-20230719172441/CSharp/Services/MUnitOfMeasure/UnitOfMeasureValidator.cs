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

namespace IWM.Services.MUnitOfMeasure
{
    public interface IUnitOfMeasureValidator : IServiceScoped
    {
        Task Get(UnitOfMeasure UnitOfMeasure);
        Task<bool> Import(List<UnitOfMeasure> UnitOfMeasures);
    }

    public class UnitOfMeasureValidator : IUnitOfMeasureValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private UnitOfMeasureMessage UnitOfMeasureMessage;

        public UnitOfMeasureValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureMessage = new UnitOfMeasureMessage();
        }

        public async Task Get(UnitOfMeasure UnitOfMeasure)
        {
        }


        public async Task<bool> Import(List<UnitOfMeasure> UnitOfMeasures)
        {
            return true;
        }
        
    }
}
