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
    public interface ITaxTypeRepository
    {
        Task<int> CountAll(TaxTypeFilter TaxTypeFilter);
        Task<int> Count(TaxTypeFilter TaxTypeFilter);
        Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter);
        Task<List<TaxType>> List(List<long> Ids);
        Task<TaxType> Get(long Id);
        Task<List<long>> BulkMerge(List<TaxType> TaxTypes);
    }
    public class TaxTypeRepository : ITaxTypeRepository
    {
        private readonly DataContext DataContext;
        public TaxTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<TaxTypeDAO>> DynamicFilter(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Percentage, filter.Percentage);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(TaxTypeSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(TaxTypeSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<TaxTypeDAO>> OrFilter(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (TaxTypeFilter TaxTypeFilter in filter.OrFilter)
            {
                IQueryable<TaxTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, TaxTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, TaxTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, TaxTypeFilter.Name);
                queryable = queryable.Where(q => q.Percentage, TaxTypeFilter.Percentage);
                queryable = queryable.Where(q => q.StatusId, TaxTypeFilter.StatusId);
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

        private IQueryable<TaxTypeDAO> DynamicOrder(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            Dictionary<TaxTypeOrder, LambdaExpression> CustomOrder = new Dictionary<TaxTypeOrder, LambdaExpression>()
            {
                { TaxTypeOrder.Status, (TaxTypeDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<TaxType>> DynamicSelect(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            List<TaxType> TaxTypes = await query.Select(q => new TaxType()
            {
                Id = filter.Selects.Contains(TaxTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(TaxTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(TaxTypeSelect.Name) ? q.Name : default(string),
                Percentage = filter.Selects.Contains(TaxTypeSelect.Percentage) ? q.Percentage : default(decimal),
                StatusId = filter.Selects.Contains(TaxTypeSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(TaxTypeSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(TaxTypeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                    Color = q.Status.Color,
                } : null,
                RowId = q.RowId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListAsync();
            return TaxTypes;
        }

        public async Task<int> CountAll(TaxTypeFilter filter)
        {
            IQueryable<TaxTypeDAO> TaxTypeDAOs = DataContext.TaxType.AsNoTracking();
            TaxTypeDAOs = await DynamicFilter(TaxTypeDAOs, filter);
            return await TaxTypeDAOs.CountAsync();
        }

        public async Task<int> Count(TaxTypeFilter filter)
        {
            IQueryable<TaxTypeDAO> TaxTypeDAOs = DataContext.TaxType.AsNoTracking();
            TaxTypeDAOs = await DynamicFilter(TaxTypeDAOs, filter);
            TaxTypeDAOs = await OrFilter(TaxTypeDAOs, filter);
            return await TaxTypeDAOs.CountAsync();
        }

        public async Task<List<TaxType>> List(TaxTypeFilter filter)
        {
            if (filter == null) return new List<TaxType>();
            IQueryable<TaxTypeDAO> TaxTypeDAOs = DataContext.TaxType.AsNoTracking();
            TaxTypeDAOs = await DynamicFilter(TaxTypeDAOs, filter);
            TaxTypeDAOs = await OrFilter(TaxTypeDAOs, filter);
            TaxTypeDAOs = DynamicOrder(TaxTypeDAOs, filter);
            List<TaxType> TaxTypes = await DynamicSelect(TaxTypeDAOs, filter);
            return TaxTypes;
        }

        public async Task<List<TaxType>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<TaxTypeDAO> query = DataContext.TaxType.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<TaxType> TaxTypes = await query.AsNoTracking()
            .Select(x => new TaxType()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Percentage = x.Percentage,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();


            return TaxTypes;
        }

        public async Task<TaxType> Get(long Id)
        {
            TaxType TaxType = await DataContext.TaxType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new TaxType()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Percentage = x.Percentage,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (TaxType == null)
                return null;

            return TaxType;
        }

        public async Task<List<long>> BulkMerge(List<TaxType> TaxTypes)
        {
            IdFilter IdFilter = new IdFilter { In = TaxTypes.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<TaxTypeDAO> TaxTypeDAOs = new List<TaxTypeDAO>();
            foreach (TaxType TaxType in TaxTypes)
            {
                TaxTypeDAO TaxTypeDAO = new TaxTypeDAO();
                TaxTypeDAOs.Add(TaxTypeDAO);
                TaxTypeDAO.Id = TaxType.Id;
                TaxTypeDAO.Code = TaxType.Code;
                TaxTypeDAO.Name = TaxType.Name;
                TaxTypeDAO.Percentage = TaxType.Percentage;
                TaxTypeDAO.StatusId = TaxType.StatusId;
                TaxTypeDAO.Used = TaxType.Used;
                TaxTypeDAO.CreatedAt = TaxType.CreatedAt;
                TaxTypeDAO.UpdatedAt = TaxType.UpdatedAt;
                TaxTypeDAO.DeletedAt = TaxType.DeletedAt;
                TaxTypeDAO.RowId = TaxType.RowId;
            }
            await DataContext.BulkMergeAsync(TaxTypeDAOs);
            var Ids = TaxTypeDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.TaxType
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new TaxTypeDAO { Used = true });
            return true;
        }
    }
}
