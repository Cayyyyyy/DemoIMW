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

        public WorkerValidator(IUOW UOW, ICurrentContext CurrentContext): base(nameof(WorkerValidator))
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
            AddError(
                entity: Worker,
                field: nameof(Worker.Id),
                error: () =>
                {
                    if (oldData != null)
                    {
                    }
                    else
                    {
                        return WorkerMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: WorkerMessage);
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
                Id = new IdFilter { Equal = Worker.Id },
                Selects = WorkerSelect.Id
            };

            int count = await UOW.WorkerRepository.CountAll(WorkerFilter);
            AddError(
                entity: Worker,
                field: nameof(Worker.Id),
                error: () =>
                {
                    if (count == 0)
                    {
                        return WorkerMessage.Error.IdNotExisted;
                    }
                    return null;
                },
                message: WorkerMessage);
            return Worker.IsValidated;
        }

        private async Task<bool> ValidateCode(Worker Worker)
        {
            WorkerFilter WorkerFilter = new WorkerFilter
            {
                Id = new IdFilter { NotEqual = Worker.Id },
                Code = new StringFilter { Equal = Worker.Code },
                Selects = WorkerSelect.Code
            };

            int count = await UOW.WorkerRepository.Count(WorkerFilter);

            AddError(
                entity: Worker,
                field: nameof(Worker.Code),
                error: () =>
                {
                    if (string.IsNullOrEmpty(Worker.Code))
                    {
                        return WorkerMessage.Error.CodeEmpty;
                    }
                    else if (Worker.Code.Length > 20)
                    {
                        return WorkerMessage.Error.CodeOverLength;
                    }
                    else
                    {
                        if (Worker.Code.Contains(" ") || Worker.Code.HasSpecialChar())
                        {
                            return WorkerMessage.Error.CodeHasSpecialCharacter;
                        }
                        else if (count != 0)
                        {
                            return WorkerMessage.Error.CodeExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return Worker.IsValidated;
        }


        private async Task<bool> ValidateName(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Name),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Name))
                    {
                        return WorkerMessage.Error.NameEmpty;
                    }
                    else if(Worker.Name.Length > 255)
                    {
                        return WorkerMessage.Error.NameOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateBirthday(Worker Worker)
        {    
            AddError(
                entity: Worker,
                field: nameof(Worker.Birthday),
                error: () =>
                {
                    if(Worker.Birthday.HasValue && Worker.Birthday <= new DateTime(2000, 1, 1))
                    {
                        return WorkerMessage.Error.BirthdayInvalid;
                    }
                    return null;
                },
                message: WorkerMessage);
                   
            return true;
        }

        private async Task<bool> ValidatePhone(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Phone),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Phone))
                    {
                        return WorkerMessage.Error.PhoneEmpty;
                    }
                    else if(Worker.Phone.Length > 255)
                    {
                        return WorkerMessage.Error.PhoneOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateCitizenIdentificationNumber(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.CitizenIdentificationNumber),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.CitizenIdentificationNumber))
                    {
                        return WorkerMessage.Error.CitizenIdentificationNumberEmpty;
                    }
                    else if(Worker.CitizenIdentificationNumber.Length > 255)
                    {
                        return WorkerMessage.Error.CitizenIdentificationNumberOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateEmail(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Email),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Email))
                    {
                        return WorkerMessage.Error.EmailEmpty;
                    }
                    else if(Worker.Email.Length > 255)
                    {
                        return WorkerMessage.Error.EmailOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateAddress(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Address),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Address))
                    {
                        return WorkerMessage.Error.AddressEmpty;
                    }
                    else if(Worker.Address.Length > 255)
                    {
                        return WorkerMessage.Error.AddressOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateUsername(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Username),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Username))
                    {
                        return WorkerMessage.Error.UsernameEmpty;
                    }
                    else if(Worker.Username.Length > 255)
                    {
                        return WorkerMessage.Error.UsernameOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidatePassword(Worker Worker)
        {
            AddError(
                entity: Worker,
                field: nameof(Worker.Password),
                error: () =>
                {
                    if(string.IsNullOrEmpty(Worker.Password))
                    {
                        return WorkerMessage.Error.PasswordEmpty;
                    }
                    else if(Worker.Password.Length > 255)
                    {
                        return WorkerMessage.Error.PasswordOverLength;
                    }
                    return null;
                },
                message: WorkerMessage);

            return Worker.IsValidated;
        }

        private async Task<bool> ValidateDistrict(Worker Worker)
        {      
            int count = await UOW.DistrictRepository.CountAll(new DistrictFilter
            {
                Id = new IdFilter{ Equal =  Worker.DistrictId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.District),
                error: () =>
                {
                    if(Worker.DistrictId.HasValue)
                    {
                        if(count == 0)
                        {
                            return WorkerMessage.Error.DistrictNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateNation(Worker Worker)
        {      
            int count = await UOW.NationRepository.CountAll(new NationFilter
            {
                Id = new IdFilter{ Equal =  Worker.NationId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.Nation),
                error: () =>
                {
                    if(Worker.NationId.HasValue)
                    {
                        if(count == 0)
                        {
                            return WorkerMessage.Error.NationNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateProvince(Worker Worker)
        {      
            int count = await UOW.ProvinceRepository.CountAll(new ProvinceFilter
            {
                Id = new IdFilter{ Equal =  Worker.ProvinceId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.Province),
                error: () =>
                {
                    if(Worker.ProvinceId.HasValue)
                    {
                        if(count == 0)
                        {
                            return WorkerMessage.Error.ProvinceNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateSex(Worker Worker)
        {      
            int count = await UOW.SexRepository.CountAll(new SexFilter
            {
                Id = new IdFilter{ Equal =  Worker.SexId },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.Sex),
                error: () =>
                {
                    if(Worker.SexId.HasValue)
                    {
                        if(!Sex.SexEnumList.Any(x => Worker.SexId == x.Id))
                        {
                            return WorkerMessage.Error.SexNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateStatus(Worker Worker)
        {      
            int count = await UOW.StatusRepository.CountAll(new StatusFilter
            {
                Id = new IdFilter{ Equal =  Worker.StatusId },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.Status),
                error: () =>
                {
                    if(Worker.StatusId == 0)
                    {
                        return WorkerMessage.Error.StatusEmpty;
                    }
                    else
                    {
                        if(!Status.StatusEnumList.Any(x => Worker.StatusId == x.Id))
                        {
                            return WorkerMessage.Error.StatusNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateWard(Worker Worker)
        {      
            int count = await UOW.WardRepository.CountAll(new WardFilter
            {
                Id = new IdFilter{ Equal =  Worker.WardId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.Ward),
                error: () =>
                {
                    if(Worker.WardId.HasValue)
                    {
                        if(count == 0)
                        {
                            return WorkerMessage.Error.WardNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }

        private async Task<bool> ValidateWorkerGroup(Worker Worker)
        {      
            int count = await UOW.WorkerGroupRepository.CountAll(new WorkerGroupFilter
            {
                Id = new IdFilter{ Equal =  Worker.WorkerGroupId },
                StatusId = new IdFilter{ Equal = Status.ACTIVE.Id },
            }); 
            AddError(
                entity: Worker,
                field: nameof(Worker.WorkerGroup),
                error: () =>
                {
                    if(Worker.WorkerGroupId.HasValue)
                    {
                        if(count == 0)
                        {
                            return WorkerMessage.Error.WorkerGroupNotExisted;
                        }
                    }
                    return null;
                },
                message: WorkerMessage);
            return true;
        }


    }
}
