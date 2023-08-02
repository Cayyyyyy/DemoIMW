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

        public UnitOfMeasureGroupingContentValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(UnitOfMeasureGroupingContentValidator))
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
            AddError(
                entity: UnitOfMeasureGroupingContent,
                field: nameof(UnitOfMeasureGroupingContent.Id),
                error: () =>
                {
                    if (oldData != null)
                    {
                    }
                    else
                    {
                        return UnitOfMeasureGroupingContentMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: UnitOfMeasureGroupingContentMessage);
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
                Id = new IdFilter { Equal = UnitOfMeasureGroupingContent.Id },
                Selects = UnitOfMeasureGroupingContentSelect.Id
            };

            int count = await UOW.UnitOfMeasureGroupingContentRepository.CountAll(UnitOfMeasureGroupingContentFilter);
            AddError(
                entity: UnitOfMeasureGroupingContent,
                field: nameof(UnitOfMeasureGroupingContent.Id),
                error: () =>
                {
                    if (count == 0)
                    {
                        return UnitOfMeasureGroupingContentMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: UnitOfMeasureGroupingContentMessage);
            return UnitOfMeasureGroupingContent.IsValidated;
        }



        private async Task<bool> ValidateFactor(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {   
            AddError(
                entity: UnitOfMeasureGroupingContent,
                field: nameof(UnitOfMeasureGroupingContent.Factor),
                error: () =>
                {
                    if(UnitOfMeasureGroupingContent.Factor.HasValue && UnitOfMeasureGroupingContent.Factor <= 0)
                    {
                        return UnitOfMeasureGroupingContentMessage.Error.FactorInvalid;
                    }
                    return null;
                },
                message: UnitOfMeasureGroupingContentMessage);

            return true;
        }


        private async Task<bool> ValidateUnitOfMeasure(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {      
            int count = await UOW.UnitOfMeasureRepository.CountAll(new UnitOfMeasureFilter
            {
                Id = new IdFilter{ Equal =  UnitOfMeasureGroupingContent.UnitOfMeasureId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: UnitOfMeasureGroupingContent,
                field: nameof(UnitOfMeasureGroupingContent.UnitOfMeasure),
                error: () =>
                {
                    if(UnitOfMeasureGroupingContent.UnitOfMeasureId == 0)
                    {
                        return UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureEmpty;
                    }
                    else
                    {
                        if(count == 0)
                        {
                            return UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureNotExisted;
                        }
                    }
                    return null;
                },
                message: UnitOfMeasureGroupingContentMessage);
            return true;
        }

        private async Task<bool> ValidateUnitOfMeasureGrouping(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {      
            int count = await UOW.UnitOfMeasureGroupingRepository.CountAll(new UnitOfMeasureGroupingFilter
            {
                Id = new IdFilter{ Equal =  UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: UnitOfMeasureGroupingContent,
                field: nameof(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping),
                error: () =>
                {
                    if(UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId == 0)
                    {
                        return UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureGroupingEmpty;
                    }
                    else
                    {
                        if(count == 0)
                        {
                            return UnitOfMeasureGroupingContentMessage.Error.UnitOfMeasureGroupingNotExisted;
                        }
                    }
                    return null;
                },
                message: UnitOfMeasureGroupingContentMessage);
            return true;
        }

    }
}
