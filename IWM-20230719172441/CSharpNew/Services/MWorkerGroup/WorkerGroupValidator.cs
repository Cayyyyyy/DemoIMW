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

namespace IWM.Services.MWorkerGroup
{
    public interface IWorkerGroupValidator : IServiceScoped
    {
        Task Get(WorkerGroup WorkerGroup);
        Task<bool> Create(WorkerGroup WorkerGroup);
        Task<bool> Update(WorkerGroup WorkerGroup);
        Task<bool> Delete(WorkerGroup WorkerGroup);
        Task<bool> BulkDelete(List<WorkerGroup> WorkerGroups);
        Task<bool> Import(List<WorkerGroup> WorkerGroups);
    }

    public class WorkerGroupValidator : IWorkerGroupValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private WorkerGroupMessage WorkerGroupMessage;

        public WorkerGroupValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(WorkerGroupValidator))
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.WorkerGroupMessage = new WorkerGroupMessage();
        }

        public async Task Get(WorkerGroup WorkerGroup)
        {
        }

        public async Task<bool> Create(WorkerGroup WorkerGroup)
        {
            await ValidateCode(WorkerGroup);
            await ValidateName(WorkerGroup);
            await ValidateStatus(WorkerGroup);
            return WorkerGroup.IsValidated;
        }

        public async Task<bool> Update(WorkerGroup WorkerGroup)
        {
            if (await ValidateId(WorkerGroup))
            {
                await ValidateCode(WorkerGroup);
                await ValidateName(WorkerGroup);
                await ValidateStatus(WorkerGroup);
            }
            return WorkerGroup.IsValidated;
        }

        public async Task<bool> Delete(WorkerGroup WorkerGroup)
        {
            var oldData = await UOW.WorkerGroupRepository.Get(WorkerGroup.Id);
            AddError(
                entity: WorkerGroup,
                field: nameof(WorkerGroup.Id),
                error: () =>
                {
                    if (oldData != null)
                    {
                    }
                    else
                    {
                        return WorkerGroupMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: WorkerGroupMessage);
            return WorkerGroup.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<WorkerGroup> WorkerGroups)
        {
            return WorkerGroups.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<WorkerGroup> WorkerGroups)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(WorkerGroup WorkerGroup)
        {
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter
            {
                Id = new IdFilter { Equal = WorkerGroup.Id },
                Selects = WorkerGroupSelect.Id
            };

            int count = await UOW.WorkerGroupRepository.CountAll(WorkerGroupFilter);
            AddError(
                entity: WorkerGroup,
                field: nameof(WorkerGroup.Id),
                error: () =>
                {
                    if (count == 0)
                    {
                        return WorkerGroupMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: WorkerGroupMessage);
            return WorkerGroup.IsValidated;
        }

        private async Task<bool> ValidateCode(WorkerGroup WorkerGroup)
        {
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter
            {
                Id = new IdFilter { NotEqual = WorkerGroup.Id },
                Code = new StringFilter { Equal = WorkerGroup.Code },
                Selects = WorkerGroupSelect.Code
            };

            int count = await UOW.WorkerGroupRepository.Count(WorkerGroupFilter);

            AddError(
                entity: WorkerGroup,
                field: nameof(WorkerGroup.Code),
                error: () =>
                {
                    if (string.IsNullOrEmpty(WorkerGroup.Code))
                    {
                        return WorkerGroupMessage.Error.CodeEmpty;
                    }
                    else if (WorkerGroup.Code.Length > 20)
                    {
                        return WorkerGroupMessage.Error.CodeOverLength;
                    }
                    else
                    {
                        if (WorkerGroup.Code.Contains(" ") || WorkerGroup.Code.HasSpecialChar())
                        {
                            return WorkerGroupMessage.Error.CodeHasSpecialCharacter;
                        }
                        else if (count != 0)
                        {
                            return WorkerGroupMessage.Error.CodeExisted;
                        }
                    }
                    return null;
                },
                message: WorkerGroupMessage);
            return WorkerGroup.IsValidated;
        }


        private async Task<bool> ValidateName(WorkerGroup WorkerGroup)
        {
            AddError(
                entity: WorkerGroup,
                field: nameof(WorkerGroup.Name),
                error: () =>
                {
                    if(string.IsNullOrEmpty(WorkerGroup.Name))
                    {
                        return WorkerGroupMessage.Error.NameEmpty;
                    }
                    else if(WorkerGroup.Name.Length > 255)
                    {
                        return WorkerGroupMessage.Error.NameOverLength;
                    }
                    return null;
                },
                message: WorkerGroupMessage);

            return WorkerGroup.IsValidated;
        }

        private async Task<bool> ValidateStatus(WorkerGroup WorkerGroup)
        {      
            int count = await UOW.StatusRepository.CountAll(new StatusFilter
            {
                Id = new IdFilter{ Equal =  WorkerGroup.StatusId },
            }); 
            AddError(
                entity: WorkerGroup,
                field: nameof(WorkerGroup.Status),
                error: () =>
                {
                    if(WorkerGroup.StatusId == 0)
                    {
                        return WorkerGroupMessage.Error.StatusEmpty;
                    }
                    else
                    {
                        if(!Status.StatusEnumList.Any(x => WorkerGroup.StatusId == x.Id))
                        {
                            return WorkerGroupMessage.Error.StatusNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerGroupMessage);
            return true;
        }


    }
}
