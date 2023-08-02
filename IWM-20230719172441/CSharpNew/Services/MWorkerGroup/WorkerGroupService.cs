using TrueSight;
using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using IWM.Repositories;
using IWM.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MWorkerGroup
{
    public interface IWorkerGroupService : IServiceScoped
    {
        Task<int> Count(WorkerGroupFilter WorkerGroupFilter);
        Task<List<WorkerGroup>> List(WorkerGroupFilter WorkerGroupFilter);
        Task<WorkerGroup> Get(long Id);
        Task<WorkerGroup> Create(WorkerGroup WorkerGroup);
        Task<WorkerGroup> Update(WorkerGroup WorkerGroup);
        Task<WorkerGroup> Delete(WorkerGroup WorkerGroup);
        Task<List<WorkerGroup>> BulkDelete(List<WorkerGroup> WorkerGroups);
        Task<List<WorkerGroup>> BulkMerge(List<WorkerGroup> WorkerGroups);
        Task<List<WorkerGroup>> Export(WorkerGroupFilter WorkerGroupFilter);
        Task<WorkerGroupFilter> ToFilter(WorkerGroupFilter WorkerGroupFilter);
    }

    public class WorkerGroupService : BaseService, IWorkerGroupService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IWorkerGroupValidator WorkerGroupValidator;
        public WorkerGroupService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IWorkerGroupValidator WorkerGroupValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.WorkerGroupValidator = WorkerGroupValidator;
        }

        public async Task<int> Count(WorkerGroupFilter WorkerGroupFilter)
        {
            try
            {
                int result = await UOW.WorkerGroupRepository.Count(WorkerGroupFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<List<WorkerGroup>> List(WorkerGroupFilter WorkerGroupFilter)
        {
            try
            {
                List<WorkerGroup> WorkerGroups = await UOW.WorkerGroupRepository.List(WorkerGroupFilter);
                return WorkerGroups;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<WorkerGroup> Get(long Id)
        {
            WorkerGroup WorkerGroup = await UOW.WorkerGroupRepository.Get(Id);
            if (WorkerGroup == null)
                return null;
            await WorkerGroupValidator.Get(WorkerGroup);
            return WorkerGroup;
        }
        
        public async Task<WorkerGroup> Create(WorkerGroup WorkerGroup)
        {
            if (!await WorkerGroupValidator.Create(WorkerGroup))
                return WorkerGroup;

            try
            {
                await UOW.WorkerGroupRepository.Create(WorkerGroup);
                WorkerGroup = await UOW.WorkerGroupRepository.Get(WorkerGroup.Id);
                Sync(new List<WorkerGroup> { WorkerGroup });
                return WorkerGroup;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }
        

        public async Task<WorkerGroup> Update(WorkerGroup WorkerGroup)
        {
            if (!await WorkerGroupValidator.Update(WorkerGroup))
                return WorkerGroup;
            try
            {
                await UOW.WorkerGroupRepository.Update(WorkerGroup);

                WorkerGroup = await UOW.WorkerGroupRepository.Get(WorkerGroup.Id);
                Sync(new List<WorkerGroup> { WorkerGroup });
                return WorkerGroup;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<WorkerGroup> Delete(WorkerGroup WorkerGroup)
        {
            if (!await WorkerGroupValidator.Delete(WorkerGroup))
                return WorkerGroup;

            try
            {
                await UOW.WorkerGroupRepository.Delete(WorkerGroup);
                var WorkerGroups = await UOW.WorkerGroupRepository.List(new List<long>{ WorkerGroup.Id });
                Sync(WorkerGroups);
                WorkerGroup = WorkerGroups.FirstOrDefault();
                
                return WorkerGroup;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<List<WorkerGroup>> BulkDelete(List<WorkerGroup> WorkerGroups)
        {
            if (!await WorkerGroupValidator.BulkDelete(WorkerGroups))
                return WorkerGroups;

            try
            {
                await UOW.WorkerGroupRepository.BulkDelete(WorkerGroups);
                var Ids = WorkerGroups.Select(x => x.Id).ToList();
                WorkerGroups = await UOW.WorkerGroupRepository.List(Ids);
                Sync(WorkerGroups);
                return WorkerGroups;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<List<WorkerGroup>> BulkMerge(List<WorkerGroup> WorkerGroups)
        {
            if (!await WorkerGroupValidator.Import(WorkerGroups))
                return WorkerGroups;
            try
            {
                var Ids = await UOW.WorkerGroupRepository.BulkMerge(WorkerGroups);
                WorkerGroups = await UOW.WorkerGroupRepository.List(Ids);
                Sync(WorkerGroups);
                return WorkerGroups;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }
        
        public async Task<List<WorkerGroup>> Export(WorkerGroupFilter WorkerGroupFilter)
        {
            try
            {
                WorkerGroupFilter.Selects = WorkerGroupSelect.Id;
                List<WorkerGroup> WorkerGroups = await UOW.WorkerGroupRepository.List(WorkerGroupFilter);
                var Ids = WorkerGroups.Select(x => x.Id).ToList();
                WorkerGroups = await UOW.WorkerGroupRepository.List(Ids);
                return WorkerGroups;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(WorkerGroupService));
            }
        }

        public async Task<WorkerGroupFilter> ToFilter(WorkerGroupFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<WorkerGroupFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                WorkerGroupFilter subFilter = new WorkerGroupFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "WorkerGroupId")
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
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

        private void Sync(List<WorkerGroup> WorkerGroups)
        {
            RabbitManager.PublishList(CurrentContext, WorkerGroups, MessageRoutingKey.WorkerGroupSync);


            
        }
    }
}
