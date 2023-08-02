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

namespace IWM.Services.MUnitOfMeasureGrouping
{
    public interface IUnitOfMeasureGroupingValidator : IServiceScoped
    {
        Task Get(UnitOfMeasureGrouping UnitOfMeasureGrouping);
        Task<bool> Import(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
    }

    public class UnitOfMeasureGroupingValidator : IUnitOfMeasureGroupingValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private UnitOfMeasureGroupingMessage UnitOfMeasureGroupingMessage;

        public UnitOfMeasureGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureGroupingMessage = new UnitOfMeasureGroupingMessage();
        }

        public async Task Get(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
        }


        public async Task<bool> Import(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            return true;
        }
        
    }
}
