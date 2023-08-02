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

        public ProvinceGroupingValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(ProvinceGroupingValidator))
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
                await ValidateParent(ProvinceGrouping);
                await ValidateStatus(ProvinceGrouping);
            }
            return ProvinceGrouping.IsValidated;
        }

        public async Task<bool> Delete(ProvinceGrouping ProvinceGrouping)
        {
            var oldData = await UOW.ProvinceGroupingRepository.Get(ProvinceGrouping.Id);
            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Id),
                error: () =>
                {
                    if (oldData != null)
                    {
                    }
                    else
                    {
                        return ProvinceGroupingMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);
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
                Id = new IdFilter { Equal = ProvinceGrouping.Id },
                Selects = ProvinceGroupingSelect.Id
            };

            int count = await UOW.ProvinceGroupingRepository.CountAll(ProvinceGroupingFilter);
            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Id),
                error: () =>
                {
                    if (count == 0)
                    {
                        return ProvinceGroupingMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);
            return ProvinceGrouping.IsValidated;
        }

        private async Task<bool> ValidateCode(ProvinceGrouping ProvinceGrouping)
        {
            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter
            {
                Id = new IdFilter { NotEqual = ProvinceGrouping.Id },
                Code = new StringFilter { Equal = ProvinceGrouping.Code },
                Selects = ProvinceGroupingSelect.Code
            };

            int count = await UOW.ProvinceGroupingRepository.Count(ProvinceGroupingFilter);

            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Code),
                error: () =>
                {
                    if (string.IsNullOrEmpty(ProvinceGrouping.Code))
                    {
                        return ProvinceGroupingMessage.Error.CodeEmpty;
                    }
                    else if (ProvinceGrouping.Code.Length > 20)
                    {
                        return ProvinceGroupingMessage.Error.CodeOverLength;
                    }
                    else
                    {
                        if (ProvinceGrouping.Code.Contains(" ") || ProvinceGrouping.Code.HasSpecialChar())
                        {
                            return ProvinceGroupingMessage.Error.CodeHasSpecialCharacter;
                        }
                        else if (count != 0)
                        {
                            return ProvinceGroupingMessage.Error.CodeExisted;
                        }
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);
            return ProvinceGrouping.IsValidated;
        }


        private async Task<bool> ValidateName(ProvinceGrouping ProvinceGrouping)
        {
            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Name),
                error: () =>
                {
                    if(string.IsNullOrEmpty(ProvinceGrouping.Name))
                    {
                        return ProvinceGroupingMessage.Error.NameEmpty;
                    }
                    else if(ProvinceGrouping.Name.Length > 255)
                    {
                        return ProvinceGroupingMessage.Error.NameOverLength;
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);

            return ProvinceGrouping.IsValidated;
        }



        private async Task<bool> ValidateParent(ProvinceGrouping ProvinceGrouping)
        {      
            int count = await UOW.ProvinceGroupingRepository.CountAll(new ProvinceGroupingFilter
            {
                Id = new IdFilter{ Equal =  ProvinceGrouping.ParentId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Parent),
                error: () =>
                {
                    if(ProvinceGrouping.ParentId.HasValue)
                    {
                        if(count == 0)
                        {
                            return ProvinceGroupingMessage.Error.ParentNotExisted;
                        }
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);
            return true;
        }

        private async Task<bool> ValidateStatus(ProvinceGrouping ProvinceGrouping)
        {      
            int count = await UOW.StatusRepository.CountAll(new StatusFilter
            {
                Id = new IdFilter{ Equal =  ProvinceGrouping.StatusId },
            }); 
            AddError(
                entity: ProvinceGrouping,
                field: nameof(ProvinceGrouping.Status),
                error: () =>
                {
                    if(ProvinceGrouping.StatusId == 0)
                    {
                        return ProvinceGroupingMessage.Error.StatusEmpty;
                    }
                    else
                    {
                        if(!Status.StatusEnumList.Any(x => ProvinceGrouping.StatusId == x.Id))
                        {
                            return ProvinceGroupingMessage.Error.StatusNotExisted;
                        }
                    }
                    return null;
                },
                message: ProvinceGroupingMessage);
            return true;
        }



    }
}
