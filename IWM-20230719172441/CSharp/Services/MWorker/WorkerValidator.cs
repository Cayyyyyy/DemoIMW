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

namespace IWM.Services.MWorker
{
    public interface IWorkerValidator : IServiceScoped
    {
        Task Get(Worker Worker);
        Task<bool> Create(Worker Worker);
        Task<bool> Update(Worker Worker);
        Task<bool> Delete(Worker Worker);
        Task<bool> BulkDelete(List<Worker> Workers);
        Task<bool> Import(List<Worker> Workers);
    }

    public class WorkerValidator : IWorkerValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private WorkerMessage WorkerMessage;

        public WorkerValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.WorkerMessage = new WorkerMessage();
        }

        public async Task Get(Worker Worker)
        {
        }

        public async Task<bool> Create(Worker Worker)
        {
            await ValidateCode(Worker);
            await ValidateName(Worker);
            await ValidateBirthday(Worker);
            await ValidatePhone(Worker);
            await ValidateCitizenIdentificationNumber(Worker);
            await ValidateEmail(Worker);
            await ValidateAddress(Worker);
            await ValidateUsername(Worker);
            await ValidatePassword(Worker);
            await ValidateDistrict(Worker);
            await ValidateNation(Worker);
            await ValidateProvince(Worker);
            await ValidateSex(Worker);
            await ValidateStatus(Worker);
            await ValidateWard(Worker);
            await ValidateWorkerGroup(Worker);
            return Worker.IsValidated;
        }

        public async Task<bool> Update(Worker Worker)
        {
            if (await ValidateId(Worker))
            {
                await ValidateCode(Worker);
                await ValidateName(Worker);
                await ValidateBirthday(Worker);
                await ValidatePhone(Worker);
                await ValidateCitizenIdentificationNumber(Worker);
                await ValidateEmail(Worker);
                await ValidateAddress(Worker);
                await ValidateUsername(Worker);
                await ValidatePassword(Worker);
                await ValidateDistrict(Worker);
                await ValidateNation(Worker);
                await ValidateProvince(Worker);
                await ValidateSex(Worker);
                await ValidateStatus(Worker);
                await ValidateWard(Worker);
                await ValidateWorkerGroup(Worker);
            }
            return Worker.IsValidated;
        }

        public async Task<bool> Delete(Worker Worker)
        {
            var oldData = await UOW.WorkerRepository.Get(Worker.Id);
            if (oldData != null)
            {
            }
            else
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Id), WorkerMessage.Error.IdNotExisted, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<Worker> Workers)
        {
            return Workers.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<Worker> Workers)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(Worker Worker)
        {
            WorkerFilter WorkerFilter = new WorkerFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Worker.Id },
                Selects = WorkerSelect.Id
            };

            int count = await UOW.WorkerRepository.CountAll(WorkerFilter);
            if (count == 0)
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Id), WorkerMessage.Error.IdNotExisted, WorkerMessage);
            return Worker.IsValidated;
        }

        private async Task<bool> ValidateCode(Worker Worker)
         {
            if (string.IsNullOrEmpty(Worker.Code))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Code), WorkerMessage.Error.CodeEmpty, WorkerMessage);
            }
            else if (Worker.Code.Length > 256)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Code), WorkerMessage.Error.CodeOverLength, WorkerMessage);
            }
            else
            {
                if (Worker.Code.Contains(" ") || Utils.HasSpecialChar(Worker.Code))
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Code), WorkerMessage.Error.CodeHasSpecialCharacter, WorkerMessage);
                }
                else
                {
                    WorkerFilter WorkerFilter = new WorkerFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = Worker.Id },
                        Code = new StringFilter { Equal = Worker.Code },
                        Selects = WorkerSelect.Code
                    };

                    int count = await UOW.WorkerRepository.Count(WorkerFilter);
                    if (count != 0)
                        Worker.AddError(nameof(WorkerValidator), nameof(Worker.Code), WorkerMessage.Error.CodeExisted);
                }
            }

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateName(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Name))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Name), WorkerMessage.Error.NameEmpty, WorkerMessage);
            }
            else if(Worker.Name.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Name), WorkerMessage.Error.NameOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateBirthday(Worker Worker)
        {       
            if(Worker.Birthday.HasValue && Worker.Birthday <= new DateTime(2000, 1, 1))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Birthday), WorkerMessage.Error.BirthdayInvalid, WorkerMessage);
            }
            return true;
        }
        private async Task<bool> ValidatePhone(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Phone))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Phone), WorkerMessage.Error.PhoneEmpty, WorkerMessage);
            }
            else if(Worker.Phone.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Phone), WorkerMessage.Error.PhoneOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateCitizenIdentificationNumber(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.CitizenIdentificationNumber))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.CitizenIdentificationNumber), WorkerMessage.Error.CitizenIdentificationNumberEmpty, WorkerMessage);
            }
            else if(Worker.CitizenIdentificationNumber.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.CitizenIdentificationNumber), WorkerMessage.Error.CitizenIdentificationNumberOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateEmail(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Email))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Email), WorkerMessage.Error.EmailEmpty, WorkerMessage);
            }
            else if(Worker.Email.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Email), WorkerMessage.Error.EmailOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateAddress(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Address))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Address), WorkerMessage.Error.AddressEmpty, WorkerMessage);
            }
            else if(Worker.Address.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Address), WorkerMessage.Error.AddressOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateUsername(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Username))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Username), WorkerMessage.Error.UsernameEmpty, WorkerMessage);
            }
            else if(Worker.Username.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Username), WorkerMessage.Error.UsernameOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidatePassword(Worker Worker)
        {
            if(string.IsNullOrEmpty(Worker.Password))
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Password), WorkerMessage.Error.PasswordEmpty, WorkerMessage);
            }
            else if(Worker.Password.Length > 500)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Password), WorkerMessage.Error.PasswordOverLength, WorkerMessage);
            }
            return Worker.IsValidated;
        }
        private async Task<bool> ValidateDistrict(Worker Worker)
        {       
            if(Worker.DistrictId.HasValue)
            {
                int count = await UOW.DistrictRepository.CountAll(new DistrictFilter
                {
                    Id = new IdFilter{ Equal =  Worker.DistrictId },
                });
                if(count == 0)
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.District), WorkerMessage.Error.DistrictNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateNation(Worker Worker)
        {       
            if(Worker.NationId.HasValue)
            {
                int count = await UOW.NationRepository.CountAll(new NationFilter
                {
                    Id = new IdFilter{ Equal =  Worker.NationId },
                });
                if(count == 0)
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Nation), WorkerMessage.Error.NationNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateProvince(Worker Worker)
        {       
            if(Worker.ProvinceId.HasValue)
            {
                int count = await UOW.ProvinceRepository.CountAll(new ProvinceFilter
                {
                    Id = new IdFilter{ Equal =  Worker.ProvinceId },
                });
                if(count == 0)
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Province), WorkerMessage.Error.ProvinceNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateSex(Worker Worker)
        {       
            if(Worker.SexId.HasValue)
            {
                if(!SexEnum.SexEnumList.Any(x => Worker.SexId == x.Id))
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Sex), WorkerMessage.Error.SexNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateStatus(Worker Worker)
        {       
            if(Worker.StatusId == 0)
            {
                Worker.AddError(nameof(WorkerValidator), nameof(Worker.Status), WorkerMessage.Error.StatusEmpty, WorkerMessage);
            }
            else
            {
                if(!StatusEnum.StatusEnumList.Any(x => Worker.StatusId == x.Id))
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Status), WorkerMessage.Error.StatusNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateWard(Worker Worker)
        {       
            if(Worker.WardId.HasValue)
            {
                int count = await UOW.WardRepository.CountAll(new WardFilter
                {
                    Id = new IdFilter{ Equal =  Worker.WardId },
                });
                if(count == 0)
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.Ward), WorkerMessage.Error.WardNotExisted, WorkerMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateWorkerGroup(Worker Worker)
        {       
            if(Worker.WorkerGroupId.HasValue)
            {
                int count = await UOW.WorkerGroupRepository.CountAll(new WorkerGroupFilter
                {
                    Id = new IdFilter{ Equal =  Worker.WorkerGroupId },
                });
                if(count == 0)
                {
                    Worker.AddError(nameof(WorkerValidator), nameof(Worker.WorkerGroup), WorkerMessage.Error.WorkerGroupNotExisted, WorkerMessage);
                }
            }
            return true;
        }
    }
}
