using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using IWM.Repositories;
using IWM.Entities;
using IWM.Enums;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MWorker
{
    public interface IWorkerService :  IServiceScoped
    {
        Task<int> Count(WorkerFilter WorkerFilter);
        Task<List<Worker>> List(WorkerFilter WorkerFilter);
        Task<Worker> Get(long Id);
        Task<Worker> Create(Worker Worker);
        Task<Worker> Update(Worker Worker);
        Task<Worker> Delete(Worker Worker);
        Task<List<Worker>> BulkDelete(List<Worker> Workers);
        Task<List<Worker>> BulkMerge(List<Worker> Workers);
        Task<List<Worker>> Export(WorkerFilter WorkerFilter);
        Task<WorkerFilter> ToFilter(WorkerFilter WorkerFilter);
    }

    public class WorkerService : BaseService, IWorkerService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IWorkerValidator WorkerValidator;

        public WorkerService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IWorkerValidator WorkerValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.WorkerValidator = WorkerValidator;
        }

        public async Task<int> Count(WorkerFilter WorkerFilter)
        {
            try
            {
                int result = await UOW.WorkerRepository.Count(WorkerFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<List<Worker>> List(WorkerFilter WorkerFilter)
        {
            try
            {
                List<Worker> Workers = await UOW.WorkerRepository.List(WorkerFilter);
                return Workers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<Worker> Get(long Id)
        {
            Worker Worker = await UOW.WorkerRepository.Get(Id);
            if (Worker == null)
                return null;
            await WorkerValidator.Get(Worker);
            return Worker;
        }
        
        public async Task<Worker> Create(Worker Worker)
        {
            if (!await WorkerValidator.Create(Worker))
                return Worker;

            try
            {
                await UOW.WorkerRepository.Create(Worker);
                Worker = await UOW.WorkerRepository.Get(Worker.Id);
                Sync(new List<Worker> { Worker });
                return Worker;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }
        

        public async Task<Worker> Update(Worker Worker)
        {
            if (!await WorkerValidator.Update(Worker))
                return Worker;
            try
            {
                await UOW.WorkerRepository.Update(Worker);

                Worker = await UOW.WorkerRepository.Get(Worker.Id);
                Sync(new List<Worker> { Worker });
                return Worker;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<Worker> Delete(Worker Worker)
        {
            if (!await WorkerValidator.Delete(Worker))
                return Worker;

            try
            {
                await UOW.WorkerRepository.Delete(Worker);
                var Workers = await UOW.WorkerRepository.List(new List<long>{ Worker.Id });
                Sync(Workers);
                Worker = Workers.FirstOrDefault();
                
                return Worker;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<List<Worker>> BulkDelete(List<Worker> Workers)
        {
            if (!await WorkerValidator.BulkDelete(Workers))
                return Workers;

            try
            {
                await UOW.WorkerRepository.BulkDelete(Workers);
                var Ids = Workers.Select(x => x.Id).ToList();
                Workers = await UOW.WorkerRepository.List(Ids);
                Sync(Workers);
                return Workers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<List<Worker>> BulkMerge(List<Worker> Workers)
        {
            if (!await WorkerValidator.Import(Workers))
                return Workers;
            try
            {
                var Ids = await UOW.WorkerRepository.BulkMerge(Workers);
                Workers = await UOW.WorkerRepository.List(Ids);
                Sync(Workers);
                return Workers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }     
        
        public async Task<List<Worker>> Export(WorkerFilter WorkerFilter)
        {
            try
            {
                WorkerFilter.Selects = WorkerSelect.Id;
                List<Worker> Workers = await UOW.WorkerRepository.List(WorkerFilter);
                var Ids = Workers.Select(x => x.Id).ToList();
                Workers = await UOW.WorkerRepository.List(Ids);
                return Workers;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerService));
            }
        }

        public async Task<WorkerFilter> ToFilter(WorkerFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<WorkerFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                WorkerFilter subFilter = new WorkerFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "WorkerId")
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Birthday))
                        subFilter.Birthday = FilterBuilder.Merge(subFilter.Birthday, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Phone))
                        subFilter.Phone = FilterBuilder.Merge(subFilter.Phone, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.CitizenIdentificationNumber))
                        subFilter.CitizenIdentificationNumber = FilterBuilder.Merge(subFilter.CitizenIdentificationNumber, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Email))
                        subFilter.Email = FilterBuilder.Merge(subFilter.Email, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Address))
                        subFilter.Address = FilterBuilder.Merge(subFilter.Address, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.SexId))
                        subFilter.SexId = FilterBuilder.Merge(subFilter.SexId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.WorkerGroupId))
                        subFilter.WorkerGroupId = FilterBuilder.Merge(subFilter.WorkerGroupId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.NationId))
                        subFilter.NationId = FilterBuilder.Merge(subFilter.NationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProvinceId))
                        subFilter.ProvinceId = FilterBuilder.Merge(subFilter.ProvinceId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.DistrictId))
                        subFilter.DistrictId = FilterBuilder.Merge(subFilter.DistrictId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.WardId))
                        subFilter.WardId = FilterBuilder.Merge(subFilter.WardId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Username))
                        subFilter.Username = FilterBuilder.Merge(subFilter.Username, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Password))
                        subFilter.Password = FilterBuilder.Merge(subFilter.Password, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }

        private void Sync(List<Worker> Workers)
        {
            RabbitManager.PublishList(Workers, MessageRoutingKey.WorkerSync);

            List<District> Districts = new List<District>();
            List<Nation> Nations = new List<Nation>();
            List<Province> Provinces = new List<Province>();
            List<Ward> Wards = new List<Ward>();

            Districts.AddRange(Workers.Where(x => x.DistrictId.HasValue).Select(x => new District { Id = x.DistrictId.Value }));
            Nations.AddRange(Workers.Where(x => x.NationId.HasValue).Select(x => new Nation { Id = x.NationId.Value }));
            Provinces.AddRange(Workers.Where(x => x.ProvinceId.HasValue).Select(x => new Province { Id = x.ProvinceId.Value }));
            Wards.AddRange(Workers.Where(x => x.WardId.HasValue).Select(x => new Ward { Id = x.WardId.Value }));
            
            Districts = Districts.Distinct().ToList();
            Nations = Nations.Distinct().ToList();
            Provinces = Provinces.Distinct().ToList();
            Wards = Wards.Distinct().ToList();
            RabbitManager.PublishList(Districts, MessageRoutingKey.DistrictUsed);
            RabbitManager.PublishList(Nations, MessageRoutingKey.NationUsed);
            RabbitManager.PublishList(Provinces, MessageRoutingKey.ProvinceUsed);
            RabbitManager.PublishList(Wards, MessageRoutingKey.WardUsed);
        }
    }
}
