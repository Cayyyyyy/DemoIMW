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
    public interface IUnitOfMeasureGroupingRepository
    {
        Task<int> CountAll(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<int> Count(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<List<UnitOfMeasureGrouping>> List(List<long> Ids);
        Task<UnitOfMeasureGrouping> Get(long Id);
        Task<List<long>> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
    }
    public class UnitOfMeasureGroupingRepository : IUnitOfMeasureGroupingRepository
    {
        private readonly DataContext DataContext;
        public UnitOfMeasureGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<UnitOfMeasureGroupingDAO>> DynamicFilter(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(UnitOfMeasureGroupingSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(UnitOfMeasureGroupingSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<UnitOfMeasureGroupingDAO>> OrFilter(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter in filter.OrFilter)
            {
                IQueryable<UnitOfMeasureGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UnitOfMeasureGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, UnitOfMeasureGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, UnitOfMeasureGroupingFilter.Name);
                queryable = queryable.Where(q => q.Description, UnitOfMeasureGroupingFilter.Description);
                queryable = queryable.Where(q => q.StatusId, UnitOfMeasureGroupingFilter.StatusId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, UnitOfMeasureGroupingFilter.UnitOfMeasureId);
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

        private IQueryable<UnitOfMeasureGroupingDAO> DynamicOrder(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            Dictionary<UnitOfMeasureGroupingOrder, LambdaExpression> CustomOrder = new Dictionary<UnitOfMeasureGroupingOrder, LambdaExpression>()
            {
                { UnitOfMeasureGroupingOrder.Status, (UnitOfMeasureGroupingDAO x) => x.Status.Name  },
                { UnitOfMeasureGroupingOrder.UnitOfMeasure, (UnitOfMeasureGroupingDAO x) => x.UnitOfMeasure.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<UnitOfMeasureGrouping>> DynamicSelect(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await query.Select(q => new UnitOfMeasureGrouping()
            {
                Id = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Description) ? q.Description : default(string),
                UnitOfMeasureId = filter.Selects.Contains(UnitOfMeasureGroupingSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                StatusId = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                    Color = q.Status.Color,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(UnitOfMeasureGroupingSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
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
                RowId = q.RowId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListAsync();
            return UnitOfMeasureGroupings;
        }

        public async Task<int> CountAll(UnitOfMeasureGroupingFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = DataContext.UnitOfMeasureGrouping.AsNoTracking();
            UnitOfMeasureGroupingDAOs = await DynamicFilter(UnitOfMeasureGroupingDAOs, filter);
            return await UnitOfMeasureGroupingDAOs.CountAsync();
        }

        public async Task<int> Count(UnitOfMeasureGroupingFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = DataContext.UnitOfMeasureGrouping.AsNoTracking();
            UnitOfMeasureGroupingDAOs = await DynamicFilter(UnitOfMeasureGroupingDAOs, filter);
            UnitOfMeasureGroupingDAOs = await OrFilter(UnitOfMeasureGroupingDAOs, filter);
            return await UnitOfMeasureGroupingDAOs.CountAsync();
        }

        public async Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter filter)
        {
            if (filter == null) return new List<UnitOfMeasureGrouping>();
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = DataContext.UnitOfMeasureGrouping.AsNoTracking();
            UnitOfMeasureGroupingDAOs = await DynamicFilter(UnitOfMeasureGroupingDAOs, filter);
            UnitOfMeasureGroupingDAOs = await OrFilter(UnitOfMeasureGroupingDAOs, filter);
            UnitOfMeasureGroupingDAOs = DynamicOrder(UnitOfMeasureGroupingDAOs, filter);
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await DynamicSelect(UnitOfMeasureGroupingDAOs, filter);
            return UnitOfMeasureGroupings;
        }

        public async Task<List<UnitOfMeasureGrouping>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<UnitOfMeasureGroupingDAO> query = DataContext.UnitOfMeasureGrouping.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await query.AsNoTracking()
            .Select(x => new UnitOfMeasureGrouping()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                UnitOfMeasureId = x.UnitOfMeasureId,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
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
            }).ToListAsync();


            return UnitOfMeasureGroupings;
        }

        public async Task<UnitOfMeasureGrouping> Get(long Id)
        {
            UnitOfMeasureGrouping UnitOfMeasureGrouping = await DataContext.UnitOfMeasureGrouping.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new UnitOfMeasureGrouping()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                UnitOfMeasureId = x.UnitOfMeasureId,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
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
            }).FirstOrDefaultAsync();

            if (UnitOfMeasureGrouping == null)
                return null;

            return UnitOfMeasureGrouping;
        }

        public async Task<List<long>> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            IdFilter IdFilter = new IdFilter { In = UnitOfMeasureGroupings.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = new List<UnitOfMeasureGroupingDAO>();
            foreach (UnitOfMeasureGrouping UnitOfMeasureGrouping in UnitOfMeasureGroupings)
            {
                UnitOfMeasureGroupingDAO UnitOfMeasureGroupingDAO = new UnitOfMeasureGroupingDAO();
                UnitOfMeasureGroupingDAOs.Add(UnitOfMeasureGroupingDAO);
                UnitOfMeasureGroupingDAO.Id = UnitOfMeasureGrouping.Id;
                UnitOfMeasureGroupingDAO.Code = UnitOfMeasureGrouping.Code;
                UnitOfMeasureGroupingDAO.Name = UnitOfMeasureGrouping.Name;
                UnitOfMeasureGroupingDAO.Description = UnitOfMeasureGrouping.Description;
                UnitOfMeasureGroupingDAO.UnitOfMeasureId = UnitOfMeasureGrouping.UnitOfMeasureId;
                UnitOfMeasureGroupingDAO.StatusId = UnitOfMeasureGrouping.StatusId;
                UnitOfMeasureGroupingDAO.Used = UnitOfMeasureGrouping.Used;
                UnitOfMeasureGroupingDAO.CreatedAt = UnitOfMeasureGrouping.CreatedAt;
                UnitOfMeasureGroupingDAO.UpdatedAt = UnitOfMeasureGrouping.UpdatedAt;
                UnitOfMeasureGroupingDAO.DeletedAt = UnitOfMeasureGrouping.DeletedAt;
                UnitOfMeasureGroupingDAO.RowId = UnitOfMeasureGrouping.RowId;
            }
            await DataContext.BulkMergeAsync(UnitOfMeasureGroupingDAOs);
            var Ids = UnitOfMeasureGroupingDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.UnitOfMeasureGrouping
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new UnitOfMeasureGroupingDAO { Used = true });
            return true;
        }
    }
}
