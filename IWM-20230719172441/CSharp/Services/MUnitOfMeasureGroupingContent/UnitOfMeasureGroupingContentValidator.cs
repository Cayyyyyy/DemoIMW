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

namespace IWM.Services.MUnitOfMeasureGroupingContent
{
    public interface IUnitOfMeasureGroupingContentValidator : IServiceScoped
    {
        Task Get(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
        Task<bool> Import(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
    }

    public class UnitOfMeasureGroupingContentValidator : IUnitOfMeasureGroupingContentValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private UnitOfMeasureGroupingContentMessage UnitOfMeasureGroupingContentMessage;

        public UnitOfMeasureGroupingContentValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureGroupingContentMessage = new UnitOfMeasureGroupingContentMessage();
        }

        public async Task Get(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
        }

        public async Task<bool> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            await ValidateFactor(UnitOfMeasureGroupingContent);
            await ValidateUnitOfMeasure(UnitOfMeasureGroupingContent);
            await ValidateUnitOfMeasureGrouping(UnitOfMeasureGroupingContent);
            return UnitOfMeasureGroupingContent.IsValidated;
        }

        public async Task<bool> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            if (await ValidateId(UnitOfMeasureGroupingContent))
            {
                await ValidateFactor(UnitOfMeasureGroupingContent);
                await ValidateUnitOfMeasure(UnitOfMeasureGroupingContent);
                await ValidateUnitOfMeasureGrouping(UnitOfMeasureGroupingContent);
            }
            return UnitOfMeasureGroupingContent.IsValidated;
        }

        public async Task<bool> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            var oldData = await UOW.UnitOfMeasureGroupingContentRepository.Get(UnitOfMeasureGroupingContent.Id);
            if (oldData != null)
            {
            }
            else
            {
                UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.Id), UnitOfMeasureGroupingContentMessage.Error.IdNotExisted, UnitOfMeasureGroupingContentMessage);
            }
            return UnitOfMeasureGroupingContent.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            return UnitOfMeasureGroupingContents.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = new UnitOfMeasureGroupingContentFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = UnitOfMeasureGroupingContent.Id },
                Selects = UnitOfMeasureGroupingContentSelect.Id
            };

            int count = await UOW.UnitOfMeasureGroupingContentRepository.CountAll(UnitOfMeasureGroupingContentFilter);
            if (count == 0)
                UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.Id), UnitOfMeasureGroupingContentMessage.Error.IdNotExisted, UnitOfMeasureGroupingContentMessage);
            return UnitOfMeasureGroupingContent.IsValidated;
        }


        private async Task<bool> ValidateFactor(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {   
            if(UnitOfMeasureGroupingContent.Factor.HasValue && UnitOfMeasureGroupingContent.Factor <= 0)
            {
                UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.Factor), UnitOfMeasureGroupingContentMessage.Error.FactorInvalid, UnitOfMeasureGroupingContentMessage);
            }
            return true;
        }
        private async Task<bool> ValidateUnitOfMeasure(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {       
            if(UnitOfMeasureGroupingContent.UnitOfMeasureId == 0)
            {
                UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasure), UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureEmpty, UnitOfMeasureGroupingContentMessage);
            }
            else
            {
                int count = await UOW.UnitOfMeasureRepository.CountAll(new UnitOfMeasureFilter
                {
                    Id = new IdFilter{ Equal =  UnitOfMeasureGroupingContent.UnitOfMeasureId },
                });
                if(count == 0)
                {
                    UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasure), UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureNotExisted, UnitOfMeasureGroupingContentMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateUnitOfMeasureGrouping(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {       
            if(UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId == 0)
            {
                UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping), UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureGroupingEmpty, UnitOfMeasureGroupingContentMessage);
            }
            else
            {
                int count = await UOW.UnitOfMeasureGroupingRepository.CountAll(new UnitOfMeasureGroupingFilter
                {
                    Id = new IdFilter{ Equal =  UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId },
                });
                if(count == 0)
                {
                    UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingContentValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping), UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureGroupingNotExisted, UnitOfMeasureGroupingContentMessage);
                }
            }
            return true;
        }
    }
}
