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

namespace IWM.Services.MUnitOfMeasureGroupingContent
{
    public interface IUnitOfMeasureGroupingContentService :  IServiceScoped
    {
        Task<int> Count(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<List<UnitOfMeasureGroupingContent>> List(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<UnitOfMeasureGroupingContent> Get(long Id);
        Task<UnitOfMeasureGroupingContent> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<UnitOfMeasureGroupingContent> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<UnitOfMeasureGroupingContent> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<List<UnitOfMeasureGroupingContent>> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
        Task<List<UnitOfMeasureGroupingContent>> BulkMerge(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
        Task<List<UnitOfMeasureGroupingContent>> Export(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<UnitOfMeasureGroupingContentFilter> ToFilter(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
    }

    public class UnitOfMeasureGroupingContentService : BaseService, IUnitOfMeasureGroupingContentService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IUnitOfMeasureGroupingContentValidator UnitOfMeasureGroupingContentValidator;

        public UnitOfMeasureGroupingContentService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IUnitOfMeasureGroupingContentValidator UnitOfMeasureGroupingContentValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.UnitOfMeasureGroupingContentValidator = UnitOfMeasureGroupingContentValidator;
        }

        public async Task<int> Count(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter)
        {
            try
            {
                int result = await UOW.UnitOfMeasureGroupingContentRepository.Count(UnitOfMeasureGroupingContentFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<List<UnitOfMeasureGroupingContent>> List(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter)
        {
            try
            {
                List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(UnitOfMeasureGroupingContentFilter);
                return UnitOfMeasureGroupingContents;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<UnitOfMeasureGroupingContent> Get(long Id)
        {
            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = await UOW.UnitOfMeasureGroupingContentRepository.Get(Id);
            if (UnitOfMeasureGroupingContent == null)
                return null;
            await UnitOfMeasureGroupingContentValidator.Get(UnitOfMeasureGroupingContent);
            return UnitOfMeasureGroupingContent;
        }
        
        public async Task<UnitOfMeasureGroupingContent> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            if (!await UnitOfMeasureGroupingContentValidator.Create(UnitOfMeasureGroupingContent))
                return UnitOfMeasureGroupingContent;

            try
            {
                await UOW.UnitOfMeasureGroupingContentRepository.Create(UnitOfMeasureGroupingContent);
                UnitOfMeasureGroupingContent = await UOW.UnitOfMeasureGroupingContentRepository.Get(UnitOfMeasureGroupingContent.Id);
                Sync(new List<UnitOfMeasureGroupingContent> { UnitOfMeasureGroupingContent });
                return UnitOfMeasureGroupingContent;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }
        

        public async Task<UnitOfMeasureGroupingContent> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            if (!await UnitOfMeasureGroupingContentValidator.Update(UnitOfMeasureGroupingContent))
                return UnitOfMeasureGroupingContent;
            try
            {
                await UOW.UnitOfMeasureGroupingContentRepository.Update(UnitOfMeasureGroupingContent);

                UnitOfMeasureGroupingContent = await UOW.UnitOfMeasureGroupingContentRepository.Get(UnitOfMeasureGroupingContent.Id);
                Sync(new List<UnitOfMeasureGroupingContent> { UnitOfMeasureGroupingContent });
                return UnitOfMeasureGroupingContent;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<UnitOfMeasureGroupingContent> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            if (!await UnitOfMeasureGroupingContentValidator.Delete(UnitOfMeasureGroupingContent))
                return UnitOfMeasureGroupingContent;

            try
            {
                await UOW.UnitOfMeasureGroupingContentRepository.Delete(UnitOfMeasureGroupingContent);
                var UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(new List<long>{ UnitOfMeasureGroupingContent.Id });
                Sync(UnitOfMeasureGroupingContents);
                UnitOfMeasureGroupingContent = UnitOfMeasureGroupingContents.FirstOrDefault();
                
                return UnitOfMeasureGroupingContent;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<List<UnitOfMeasureGroupingContent>> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            if (!await UnitOfMeasureGroupingContentValidator.BulkDelete(UnitOfMeasureGroupingContents))
                return UnitOfMeasureGroupingContents;

            try
            {
                await UOW.UnitOfMeasureGroupingContentRepository.BulkDelete(UnitOfMeasureGroupingContents);
                var Ids = UnitOfMeasureGroupingContents.Select(x => x.Id).ToList();
                UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(Ids);
                Sync(UnitOfMeasureGroupingContents);
                return UnitOfMeasureGroupingContents;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<List<UnitOfMeasureGroupingContent>> BulkMerge(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            if (!await UnitOfMeasureGroupingContentValidator.Import(UnitOfMeasureGroupingContents))
                return UnitOfMeasureGroupingContents;
            try
            {
                var Ids = await UOW.UnitOfMeasureGroupingContentRepository.BulkMerge(UnitOfMeasureGroupingContents);
                UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(Ids);
                Sync(UnitOfMeasureGroupingContents);
                return UnitOfMeasureGroupingContents;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }     
        
        public async Task<List<UnitOfMeasureGroupingContent>> Export(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter)
        {
            try
            {
                UnitOfMeasureGroupingContentFilter.Selects = UnitOfMeasureGroupingContentSelect.Id;
                List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(UnitOfMeasureGroupingContentFilter);
                var Ids = UnitOfMeasureGroupingContents.Select(x => x.Id).ToList();
                UnitOfMeasureGroupingContents = await UOW.UnitOfMeasureGroupingContentRepository.List(Ids);
                return UnitOfMeasureGroupingContents;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(UnitOfMeasureGroupingContentService));
            }
        }

        public async Task<UnitOfMeasureGroupingContentFilter> ToFilter(UnitOfMeasureGroupingContentFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<UnitOfMeasureGroupingContentFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                UnitOfMeasureGroupingContentFilter subFilter = new UnitOfMeasureGroupingContentFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "UnitOfMeasureGroupingContentId")
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.UnitOfMeasureGroupingId))
                        subFilter.UnitOfMeasureGroupingId = FilterBuilder.Merge(subFilter.UnitOfMeasureGroupingId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.UnitOfMeasureId))
                        subFilter.UnitOfMeasureId = FilterBuilder.Merge(subFilter.UnitOfMeasureId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Factor))
                        subFilter.Factor = FilterBuilder.Merge(subFilter.Factor, FilterPermissionDefinition.DecimalFilter);
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

        private void Sync(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            RabbitManager.PublishList(UnitOfMeasureGroupingContents, MessageRoutingKey.UnitOfMeasureGroupingContentSync);

            List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = new List<UnitOfMeasureGrouping>();

            UnitOfMeasures.AddRange(UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasureGroupings.AddRange(UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasureGrouping { Id = x.UnitOfMeasureGroupingId }));
            
            UnitOfMeasures = UnitOfMeasures.Distinct().ToList();
            UnitOfMeasureGroupings = UnitOfMeasureGroupings.Distinct().ToList();
            RabbitManager.PublishList(UnitOfMeasures, MessageRoutingKey.UnitOfMeasureUsed);
            RabbitManager.PublishList(UnitOfMeasureGroupings, MessageRoutingKey.UnitOfMeasureGroupingUsed);
        }
    }
}
