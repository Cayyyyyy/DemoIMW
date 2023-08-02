using TrueSight;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using System.Linq.Expressions;

namespace IWM.Repositories
{
    public interface IUnitOfMeasureGroupingContentRepository
    {
        Task<int> CountAll(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<int> Count(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<List<UnitOfMeasureGroupingContent>> List(UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter);
        Task<List<UnitOfMeasureGroupingContent>> List(List<long> Ids);
        Task<UnitOfMeasureGroupingContent> Get(long Id);
        Task<bool> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<bool> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent);
        Task<List<long>> BulkMerge(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
        Task<bool> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents);
    }
    public class UnitOfMeasureGroupingContentRepository : IUnitOfMeasureGroupingContentRepository
    {
        private readonly DataContext DataContext;
        public UnitOfMeasureGroupingContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<UnitOfMeasureGroupingContentDAO>> DynamicFilter(IQueryable<UnitOfMeasureGroupingContentDAO> query, UnitOfMeasureGroupingContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Factor, filter.Factor);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.UnitOfMeasureGroupingId, filter.UnitOfMeasureGroupingId);

            return query;
        }

        private async Task<IQueryable<UnitOfMeasureGroupingContentDAO>> OrFilter(IQueryable<UnitOfMeasureGroupingContentDAO> query, UnitOfMeasureGroupingContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter in filter.OrFilter)
            {
                IQueryable<UnitOfMeasureGroupingContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UnitOfMeasureGroupingContentFilter.Id);
                queryable = queryable.Where(q => q.Factor, UnitOfMeasureGroupingContentFilter.Factor);
                queryable = queryable.Where(q => q.UnitOfMeasureId, UnitOfMeasureGroupingContentFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.UnitOfMeasureGroupingId, UnitOfMeasureGroupingContentFilter.UnitOfMeasureGroupingId);
                IQueryable<long> IdsQuery = queryable.Select(x => x.Id);
                Queries.Add(IdsQuery);
            }
            IQueryable<long> OrFilterQuery = query.Where(x => false).Select(x => x.Id);
            foreach (var q in Queries)
            {
                OrFilterQuery = OrFilterQuery.Union(q);
            }
            OrFilterQuery = OrFilterQuery.Distinct();
            query = from q in query
                    join o in OrFilterQuery on q.Id equals o
                    select q;
            return query;
        }

        private IQueryable<UnitOfMeasureGroupingContentDAO> DynamicOrder(IQueryable<UnitOfMeasureGroupingContentDAO> query, UnitOfMeasureGroupingContentFilter filter)
        {
            Dictionary<UnitOfMeasureGroupingContentOrder, LambdaExpression> CustomOrder = new Dictionary<UnitOfMeasureGroupingContentOrder, LambdaExpression>()
            {
                { UnitOfMeasureGroupingContentOrder.UnitOfMeasure, (UnitOfMeasureGroupingContentDAO x) => x.UnitOfMeasure.Name  },
                { UnitOfMeasureGroupingContentOrder.UnitOfMeasureGrouping, (UnitOfMeasureGroupingContentDAO x) => x.UnitOfMeasureGrouping.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<UnitOfMeasureGroupingContent>> DynamicSelect(IQueryable<UnitOfMeasureGroupingContentDAO> query, UnitOfMeasureGroupingContentFilter filter)
        {
            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await query.Select(q => new UnitOfMeasureGroupingContent()
            {
                Id = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.Id) ? q.Id : default(long),
                UnitOfMeasureGroupingId = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.UnitOfMeasureGrouping) ? q.UnitOfMeasureGroupingId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Factor = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.Factor) ? q.Factor : default(decimal?),
                UnitOfMeasure = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    IsDecimal = q.UnitOfMeasure.IsDecimal,
                    Used = q.UnitOfMeasure.Used,
                    ErpCode = q.UnitOfMeasure.ErpCode,
                } : null,
                UnitOfMeasureGrouping = filter.Selects.Contains(UnitOfMeasureGroupingContentSelect.UnitOfMeasureGrouping) && q.UnitOfMeasureGrouping != null ? new UnitOfMeasureGrouping
                {
                    Id = q.UnitOfMeasureGrouping.Id,
                    Code = q.UnitOfMeasureGrouping.Code,
                    Name = q.UnitOfMeasureGrouping.Name,
                    Description = q.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = q.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = q.UnitOfMeasureGrouping.StatusId,
                    Used = q.UnitOfMeasureGrouping.Used,
                } : null,
                RowId = q.RowId,
            }).ToListAsync();
            return UnitOfMeasureGroupingContents;
        }

        public async Task<int> CountAll(UnitOfMeasureGroupingContentFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs = DataContext.UnitOfMeasureGroupingContent.AsNoTracking();
            UnitOfMeasureGroupingContentDAOs = await DynamicFilter(UnitOfMeasureGroupingContentDAOs, filter);
            return await UnitOfMeasureGroupingContentDAOs.CountAsync();
        }

        public async Task<int> Count(UnitOfMeasureGroupingContentFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs = DataContext.UnitOfMeasureGroupingContent.AsNoTracking();
            UnitOfMeasureGroupingContentDAOs = await DynamicFilter(UnitOfMeasureGroupingContentDAOs, filter);
            UnitOfMeasureGroupingContentDAOs = await OrFilter(UnitOfMeasureGroupingContentDAOs, filter);
            return await UnitOfMeasureGroupingContentDAOs.CountAsync();
        }

        public async Task<List<UnitOfMeasureGroupingContent>> List(UnitOfMeasureGroupingContentFilter filter)
        {
            if (filter == null) return new List<UnitOfMeasureGroupingContent>();
            IQueryable<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs = DataContext.UnitOfMeasureGroupingContent.AsNoTracking();
            UnitOfMeasureGroupingContentDAOs = await DynamicFilter(UnitOfMeasureGroupingContentDAOs, filter);
            UnitOfMeasureGroupingContentDAOs = await OrFilter(UnitOfMeasureGroupingContentDAOs, filter);
            UnitOfMeasureGroupingContentDAOs = DynamicOrder(UnitOfMeasureGroupingContentDAOs, filter);
            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await DynamicSelect(UnitOfMeasureGroupingContentDAOs, filter);
            return UnitOfMeasureGroupingContents;
        }

        public async Task<List<UnitOfMeasureGroupingContent>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<UnitOfMeasureGroupingContentDAO> query = DataContext.UnitOfMeasureGroupingContent.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await query.AsNoTracking()
            .Select(x => new UnitOfMeasureGroupingContent()
            {
                RowId = x.RowId,
                Id = x.Id,
                UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Factor = x.Factor,
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    IsDecimal = x.UnitOfMeasure.IsDecimal,
                    Used = x.UnitOfMeasure.Used,
                    ErpCode = x.UnitOfMeasure.ErpCode,
                },
                UnitOfMeasureGrouping = x.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                {
                    Id = x.UnitOfMeasureGrouping.Id,
                    Code = x.UnitOfMeasureGrouping.Code,
                    Name = x.UnitOfMeasureGrouping.Name,
                    Description = x.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = x.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = x.UnitOfMeasureGrouping.StatusId,
                    Used = x.UnitOfMeasureGrouping.Used,
                },
            }).ToListAsync();


            return UnitOfMeasureGroupingContents;
        }

        public async Task<UnitOfMeasureGroupingContent> Get(long Id)
        {
            UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = await DataContext.UnitOfMeasureGroupingContent.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new UnitOfMeasureGroupingContent()
            {
                RowId = x.RowId,
                Id = x.Id,
                UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Factor = x.Factor,
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    IsDecimal = x.UnitOfMeasure.IsDecimal,
                    Used = x.UnitOfMeasure.Used,
                    ErpCode = x.UnitOfMeasure.ErpCode,
                },
                UnitOfMeasureGrouping = x.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                {
                    Id = x.UnitOfMeasureGrouping.Id,
                    Code = x.UnitOfMeasureGrouping.Code,
                    Name = x.UnitOfMeasureGrouping.Name,
                    Description = x.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = x.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = x.UnitOfMeasureGrouping.StatusId,
                    Used = x.UnitOfMeasureGrouping.Used,
                },
            }).FirstOrDefaultAsync();

            if (UnitOfMeasureGroupingContent == null)
                return null;

            return UnitOfMeasureGroupingContent;
        }
        public async Task<bool> Create(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            UnitOfMeasureGroupingContentDAO UnitOfMeasureGroupingContentDAO = new UnitOfMeasureGroupingContentDAO();
            UnitOfMeasureGroupingContentDAO.Id = UnitOfMeasureGroupingContent.Id;
            UnitOfMeasureGroupingContentDAO.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            UnitOfMeasureGroupingContentDAO.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            UnitOfMeasureGroupingContentDAO.Factor = UnitOfMeasureGroupingContent.Factor;
            UnitOfMeasureGroupingContentDAO.RowId = Guid.NewGuid();
            DataContext.UnitOfMeasureGroupingContent.Add(UnitOfMeasureGroupingContentDAO);
            await DataContext.SaveChangesAsync();
            UnitOfMeasureGroupingContent.Id = UnitOfMeasureGroupingContentDAO.Id;
            await SaveReference(UnitOfMeasureGroupingContent);
            return true;
        }

        public async Task<bool> Update(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            UnitOfMeasureGroupingContentDAO UnitOfMeasureGroupingContentDAO = DataContext.UnitOfMeasureGroupingContent
                .Where(x => x.Id == UnitOfMeasureGroupingContent.Id)
                .FirstOrDefault();
            if (UnitOfMeasureGroupingContentDAO == null)
                return false;
            UnitOfMeasureGroupingContentDAO.Id = UnitOfMeasureGroupingContent.Id;
            UnitOfMeasureGroupingContentDAO.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            UnitOfMeasureGroupingContentDAO.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            UnitOfMeasureGroupingContentDAO.Factor = UnitOfMeasureGroupingContent.Factor;
            await DataContext.SaveChangesAsync();
            await SaveReference(UnitOfMeasureGroupingContent);
            return true;
        }

        public async Task<bool> Delete(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            await DataContext.UnitOfMeasureGroupingContent
                .Where(x => x.Id == UnitOfMeasureGroupingContent.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<List<long>> BulkMerge(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            IdFilter IdFilter = new IdFilter { In = UnitOfMeasureGroupingContents.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs = new List<UnitOfMeasureGroupingContentDAO>();
            foreach (UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent in UnitOfMeasureGroupingContents)
            {
                UnitOfMeasureGroupingContentDAO UnitOfMeasureGroupingContentDAO = new UnitOfMeasureGroupingContentDAO();
                UnitOfMeasureGroupingContentDAOs.Add(UnitOfMeasureGroupingContentDAO);
                UnitOfMeasureGroupingContentDAO.Id = UnitOfMeasureGroupingContent.Id;
                UnitOfMeasureGroupingContentDAO.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
                UnitOfMeasureGroupingContentDAO.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
                UnitOfMeasureGroupingContentDAO.Factor = UnitOfMeasureGroupingContent.Factor;
                UnitOfMeasureGroupingContentDAO.RowId = UnitOfMeasureGroupingContent.RowId;
            }
            await DataContext.BulkMergeAsync(UnitOfMeasureGroupingContentDAOs);
            var Ids = UnitOfMeasureGroupingContentDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        
        public async Task<bool> BulkDelete(List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents)
        {
            List<long> Ids = UnitOfMeasureGroupingContents.Select(x => x.Id).ToList();
            await DataContext.UnitOfMeasureGroupingContent
                .WhereBulkContains(Ids, x => x.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
        }

    }
}
