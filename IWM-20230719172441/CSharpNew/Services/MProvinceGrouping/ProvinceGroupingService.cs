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

namespace IWM.Services.MProvinceGrouping
{
    public interface IProvinceGroupingService : IServiceScoped
    {
        Task<int> Count(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<List<ProvinceGrouping>> List(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<ProvinceGrouping> Get(long Id);
        Task<ProvinceGrouping> Create(ProvinceGrouping ProvinceGrouping);
        Task<ProvinceGrouping> Update(ProvinceGrouping ProvinceGrouping);
        Task<ProvinceGrouping> Delete(ProvinceGrouping ProvinceGrouping);
        Task<List<ProvinceGrouping>> BulkDelete(List<ProvinceGrouping> ProvinceGroupings);
        Task<List<ProvinceGrouping>> BulkMerge(List<ProvinceGrouping> ProvinceGroupings);
        Task<List<ProvinceGrouping>> Export(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<ProvinceGroupingFilter> ToFilter(ProvinceGroupingFilter ProvinceGroupingFilter);
    }

    public class ProvinceGroupingService : BaseService, IProvinceGroupingService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IProvinceGroupingValidator ProvinceGroupingValidator;
        public ProvinceGroupingService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IProvinceGroupingValidator ProvinceGroupingValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.ProvinceGroupingValidator = ProvinceGroupingValidator;
        }

        public async Task<int> Count(ProvinceGroupingFilter ProvinceGroupingFilter)
        {
            try
            {
                int result = await UOW.ProvinceGroupingRepository.Count(ProvinceGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<List<ProvinceGrouping>> List(ProvinceGroupingFilter ProvinceGroupingFilter)
        {
            try
            {
                List<ProvinceGrouping> ProvinceGroupings = await UOW.ProvinceGroupingRepository.List(ProvinceGroupingFilter);
                return ProvinceGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<ProvinceGrouping> Get(long Id)
        {
            ProvinceGrouping ProvinceGrouping = await UOW.ProvinceGroupingRepository.Get(Id);
            if (ProvinceGrouping == null)
                return null;
            await ProvinceGroupingValidator.Get(ProvinceGrouping);
            return ProvinceGrouping;
        }
        
        public async Task<ProvinceGrouping> Create(ProvinceGrouping ProvinceGrouping)
        {
            if (!await ProvinceGroupingValidator.Create(ProvinceGrouping))
                return ProvinceGrouping;

            try
            {
                await UOW.ProvinceGroupingRepository.Create(ProvinceGrouping);
                ProvinceGrouping = await UOW.ProvinceGroupingRepository.Get(ProvinceGrouping.Id);
                Sync(new List<ProvinceGrouping> { ProvinceGrouping });
                return ProvinceGrouping;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }
        

        public async Task<ProvinceGrouping> Update(ProvinceGrouping ProvinceGrouping)
        {
            if (!await ProvinceGroupingValidator.Update(ProvinceGrouping))
                return ProvinceGrouping;
            try
            {
                await UOW.ProvinceGroupingRepository.Update(ProvinceGrouping);

                ProvinceGrouping = await UOW.ProvinceGroupingRepository.Get(ProvinceGrouping.Id);
                Sync(new List<ProvinceGrouping> { ProvinceGrouping });
                return ProvinceGrouping;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<ProvinceGrouping> Delete(ProvinceGrouping ProvinceGrouping)
        {
            if (!await ProvinceGroupingValidator.Delete(ProvinceGrouping))
                return ProvinceGrouping;

            try
            {
                await UOW.ProvinceGroupingRepository.Delete(ProvinceGrouping);
                ProvinceGrouping.UpdatedAt = StaticParams.DateTimeNow;
                Sync(new List<ProvinceGrouping>{ ProvinceGrouping });
                
                return ProvinceGrouping;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<List<ProvinceGrouping>> BulkDelete(List<ProvinceGrouping> ProvinceGroupings)
        {
            if (!await ProvinceGroupingValidator.BulkDelete(ProvinceGroupings))
                return ProvinceGroupings;

            try
            {
                await UOW.ProvinceGroupingRepository.BulkDelete(ProvinceGroupings);
                var Ids = ProvinceGroupings.Select(x => x.Id).ToList();
                ProvinceGroupings = await UOW.ProvinceGroupingRepository.List(Ids);
                Sync(ProvinceGroupings);
                return ProvinceGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<List<ProvinceGrouping>> BulkMerge(List<ProvinceGrouping> ProvinceGroupings)
        {
            if (!await ProvinceGroupingValidator.Import(ProvinceGroupings))
                return ProvinceGroupings;
            try
            {
                var Ids = await UOW.ProvinceGroupingRepository.BulkMerge(ProvinceGroupings);
                ProvinceGroupings = await UOW.ProvinceGroupingRepository.List(Ids);
                Sync(ProvinceGroupings);
                return ProvinceGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }
        
        public async Task<List<ProvinceGrouping>> Export(ProvinceGroupingFilter ProvinceGroupingFilter)
        {
            try
            {
                ProvinceGroupingFilter.Selects = ProvinceGroupingSelect.Id;
                List<ProvinceGrouping> ProvinceGroupings = await UOW.ProvinceGroupingRepository.List(ProvinceGroupingFilter);
                var Ids = ProvinceGroupings.Select(x => x.Id).ToList();
                ProvinceGroupings = await UOW.ProvinceGroupingRepository.List(Ids);
                return ProvinceGroupings;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ProvinceGroupingService));
            }
        }

        public async Task<ProvinceGroupingFilter> ToFilter(ProvinceGroupingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ProvinceGroupingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ProvinceGroupingFilter subFilter = new ProvinceGroupingFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "ProvinceGroupingId")
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ParentId))
                        subFilter.ParentId = FilterBuilder.Merge(subFilter.ParentId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Level))
                        subFilter.Level = FilterBuilder.Merge(subFilter.Level, FilterPermissionDefinition.LongFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Path))
                        subFilter.Path = FilterBuilder.Merge(subFilter.Path, FilterPermissionDefinition.StringFilter);
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

        private void Sync(List<ProvinceGrouping> ProvinceGroupings)
        {
            RabbitManager.PublishList(CurrentContext, ProvinceGroupings, MessageRoutingKey.ProvinceGroupingSync);


            
        }
    }
}
