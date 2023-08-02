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

        public WorkerGroupValidator(IUOW UOW, ICurrentContext CurrentContext)
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
            if (oldData != null)
            {
            }
            else
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Id), WorkerGroupMessage.Error.IdNotExisted, WorkerGroupMessage);
            }
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
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = WorkerGroup.Id },
                Selects = WorkerGroupSelect.Id
            };

            int count = await UOW.WorkerGroupRepository.CountAll(WorkerGroupFilter);
            if (count == 0)
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Id), WorkerGroupMessage.Error.IdNotExisted, WorkerGroupMessage);
            return WorkerGroup.IsValidated;
        }

        private async Task<bool> ValidateCode(WorkerGroup WorkerGroup)
         {
            if (string.IsNullOrEmpty(WorkerGroup.Code))
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Code), WorkerGroupMessage.Error.CodeEmpty, WorkerGroupMessage);
            }
            else if (WorkerGroup.Code.Length > 256)
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Code), WorkerGroupMessage.Error.CodeOverLength, WorkerGroupMessage);
            }
            else
            {
                if (WorkerGroup.Code.Contains(" ") || Utils.HasSpecialChar(WorkerGroup.Code))
                {
                    WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Code), WorkerGroupMessage.Error.CodeHasSpecialCharacter, WorkerGroupMessage);
                }
                else
                {
                    WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = WorkerGroup.Id },
                        Code = new StringFilter { Equal = WorkerGroup.Code },
                        Selects = WorkerGroupSelect.Code
                    };

                    int count = await UOW.WorkerGroupRepository.Count(WorkerGroupFilter);
                    if (count != 0)
                        WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Code), WorkerGroupMessage.Error.CodeExisted);
                }
            }

            return WorkerGroup.IsValidated;
        }

        private async Task<bool> ValidateName(WorkerGroup WorkerGroup)
        {
            if(string.IsNullOrEmpty(WorkerGroup.Name))
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Name), WorkerGroupMessage.Error.NameEmpty, WorkerGroupMessage);
            }
            else if(WorkerGroup.Name.Length > 500)
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Name), WorkerGroupMessage.Error.NameOverLength, WorkerGroupMessage);
            }
            return WorkerGroup.IsValidated;
        }
        private async Task<bool> ValidateStatus(WorkerGroup WorkerGroup)
        {       
            if(WorkerGroup.StatusId == 0)
            {
                WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Status), WorkerGroupMessage.Error.StatusEmpty, WorkerGroupMessage);
            }
            else
            {
                if(!StatusEnum.StatusEnumList.Any(x => WorkerGroup.StatusId == x.Id))
                {
                    WorkerGroup.AddError(nameof(WorkerGroupValidator), nameof(WorkerGroup.Status), WorkerGroupMessage.Error.StatusNotExisted, WorkerGroupMessage);
                }
            }
            return true;
        }
    }
}
