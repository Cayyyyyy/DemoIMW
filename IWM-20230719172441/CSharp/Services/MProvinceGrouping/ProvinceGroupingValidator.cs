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

namespace IWM.Services.MProvinceGrouping
{
    public interface IProvinceGroupingValidator : IServiceScoped
    {
        Task Get(ProvinceGrouping ProvinceGrouping);
        Task<bool> Create(ProvinceGrouping ProvinceGrouping);
        Task<bool> Update(ProvinceGrouping ProvinceGrouping);
        Task<bool> Delete(ProvinceGrouping ProvinceGrouping);
        Task<bool> BulkDelete(List<ProvinceGrouping> ProvinceGroupings);
        Task<bool> Import(List<ProvinceGrouping> ProvinceGroupings);
    }

    public class ProvinceGroupingValidator : IProvinceGroupingValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private ProvinceGroupingMessage ProvinceGroupingMessage;

        public ProvinceGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.ProvinceGroupingMessage = new ProvinceGroupingMessage();
        }

        public async Task Get(ProvinceGrouping ProvinceGrouping)
        {
        }

        public async Task<bool> Create(ProvinceGrouping ProvinceGrouping)
        {
            await ValidateCode(ProvinceGrouping);
            await ValidateName(ProvinceGrouping);
            await ValidateLevel(ProvinceGrouping);
            await ValidatePath(ProvinceGrouping);
            await ValidateParent(ProvinceGrouping);
            await ValidateStatus(ProvinceGrouping);
            return ProvinceGrouping.IsValidated;
        }

        public async Task<bool> Update(ProvinceGrouping ProvinceGrouping)
        {
            if (await ValidateId(ProvinceGrouping))
            {
                await ValidateCode(ProvinceGrouping);
                await ValidateName(ProvinceGrouping);
                await ValidateLevel(ProvinceGrouping);
                await ValidatePath(ProvinceGrouping);
                await ValidateParent(ProvinceGrouping);
                await ValidateStatus(ProvinceGrouping);
            }
            return ProvinceGrouping.IsValidated;
        }

        public async Task<bool> Delete(ProvinceGrouping ProvinceGrouping)
        {
            var oldData = await UOW.ProvinceGroupingRepository.Get(ProvinceGrouping.Id);
            if (oldData != null)
            {
            }
            else
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Id), ProvinceGroupingMessage.Error.IdNotExisted, ProvinceGroupingMessage);
            }
            return ProvinceGrouping.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<ProvinceGrouping> ProvinceGroupings)
        {
            return ProvinceGroupings.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ProvinceGrouping> ProvinceGroupings)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(ProvinceGrouping ProvinceGrouping)
        {
            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ProvinceGrouping.Id },
                Selects = ProvinceGroupingSelect.Id
            };

            int count = await UOW.ProvinceGroupingRepository.CountAll(ProvinceGroupingFilter);
            if (count == 0)
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Id), ProvinceGroupingMessage.Error.IdNotExisted, ProvinceGroupingMessage);
            return ProvinceGrouping.IsValidated;
        }

        private async Task<bool> ValidateCode(ProvinceGrouping ProvinceGrouping)
         {
            if (string.IsNullOrEmpty(ProvinceGrouping.Code))
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Code), ProvinceGroupingMessage.Error.CodeEmpty, ProvinceGroupingMessage);
            }
            else if (ProvinceGrouping.Code.Length > 256)
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Code), ProvinceGroupingMessage.Error.CodeOverLength, ProvinceGroupingMessage);
            }
            else
            {
                if (ProvinceGrouping.Code.Contains(" ") || Utils.HasSpecialChar(ProvinceGrouping.Code))
                {
                    ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Code), ProvinceGroupingMessage.Error.CodeHasSpecialCharacter, ProvinceGroupingMessage);
                }
                else
                {
                    ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = ProvinceGrouping.Id },
                        Code = new StringFilter { Equal = ProvinceGrouping.Code },
                        Selects = ProvinceGroupingSelect.Code
                    };

                    int count = await UOW.ProvinceGroupingRepository.Count(ProvinceGroupingFilter);
                    if (count != 0)
                        ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Code), ProvinceGroupingMessage.Error.CodeExisted);
                }
            }

            return ProvinceGrouping.IsValidated;
        }

        private async Task<bool> ValidateName(ProvinceGrouping ProvinceGrouping)
        {
            if(string.IsNullOrEmpty(ProvinceGrouping.Name))
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Name), ProvinceGroupingMessage.Error.NameEmpty, ProvinceGroupingMessage);
            }
            else if(ProvinceGrouping.Name.Length > 500)
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Name), ProvinceGroupingMessage.Error.NameOverLength, ProvinceGroupingMessage);
            }
            return ProvinceGrouping.IsValidated;
        }
        private async Task<bool> ValidateLevel(ProvinceGrouping ProvinceGrouping)
        {   
            if(ProvinceGrouping.Level <= 0)
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Level), ProvinceGroupingMessage.Error.LevelInvalid, ProvinceGroupingMessage);
            }
            return true;
        }
        private async Task<bool> ValidatePath(ProvinceGrouping ProvinceGrouping)
        {
            if(string.IsNullOrEmpty(ProvinceGrouping.Path))
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Path), ProvinceGroupingMessage.Error.PathEmpty, ProvinceGroupingMessage);
            }
            else if(ProvinceGrouping.Path.Length > 500)
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Path), ProvinceGroupingMessage.Error.PathOverLength, ProvinceGroupingMessage);
            }
            return ProvinceGrouping.IsValidated;
        }
        private async Task<bool> ValidateParent(ProvinceGrouping ProvinceGrouping)
        {       
            if(ProvinceGrouping.ParentId.HasValue)
            {
                int count = await UOW.ProvinceGroupingRepository.CountAll(new ProvinceGroupingFilter
                {
                    Id = new IdFilter{ Equal =  ProvinceGrouping.ParentId },
                });
                if(count == 0)
                {
                    ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Parent), ProvinceGroupingMessage.Error.ParentNotExisted, ProvinceGroupingMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateStatus(ProvinceGrouping ProvinceGrouping)
        {       
            if(ProvinceGrouping.StatusId == 0)
            {
                ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Status), ProvinceGroupingMessage.Error.StatusEmpty, ProvinceGroupingMessage);
            }
            else
            {
                if(!StatusEnum.StatusEnumList.Any(x => ProvinceGrouping.StatusId == x.Id))
                {
                    ProvinceGrouping.AddError(nameof(ProvinceGroupingValidator), nameof(ProvinceGrouping.Status), ProvinceGroupingMessage.Error.StatusNotExisted, ProvinceGroupingMessage);
                }
            }
            return true;
        }
    }
}
