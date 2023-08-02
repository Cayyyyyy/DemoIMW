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
    public interface IUnitOfMeasureRepository
    {
        Task<int> CountAll(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<List<UnitOfMeasure>> List(List<long> Ids);
        Task<UnitOfMeasure> Get(long Id);
        Task<List<long>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures);
    }
    public class UnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        private readonly DataContext DataContext;
        public UnitOfMeasureRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<UnitOfMeasureDAO>> DynamicFilter(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
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
            query = query.Where(q => q.IsDecimal, filter.IsDecimal);
            query = query.Where(q => q.ErpCode, filter.ErpCode);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(UnitOfMeasureSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(UnitOfMeasureSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<UnitOfMeasureDAO>> OrFilter(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (UnitOfMeasureFilter UnitOfMeasureFilter in filter.OrFilter)
            {
                IQueryable<UnitOfMeasureDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UnitOfMeasureFilter.Id);
                queryable = queryable.Where(q => q.Code, UnitOfMeasureFilter.Code);
                queryable = queryable.Where(q => q.Name, UnitOfMeasureFilter.Name);
                queryable = queryable.Where(q => q.Description, UnitOfMeasureFilter.Description);
                queryable = queryable.Where(q => q.IsDecimal, UnitOfMeasureFilter.IsDecimal);
                queryable = queryable.Where(q => q.ErpCode, UnitOfMeasureFilter.ErpCode);
                queryable = queryable.Where(q => q.StatusId, UnitOfMeasureFilter.StatusId);
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

        private IQueryable<UnitOfMeasureDAO> DynamicOrder(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            Dictionary<UnitOfMeasureOrder, LambdaExpression> CustomOrder = new Dictionary<UnitOfMeasureOrder, LambdaExpression>()
            {
                { UnitOfMeasureOrder.Status, (UnitOfMeasureDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<UnitOfMeasure>> DynamicSelect(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            List<UnitOfMeasure> UnitOfMeasures = await query.Select(q => new UnitOfMeasure()
            {
                Id = filter.Selects.Contains(UnitOfMeasureSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(UnitOfMeasureSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(UnitOfMeasureSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(UnitOfMeasureSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(UnitOfMeasureSelect.Status) ? q.StatusId : default(long),
                IsDecimal = filter.Selects.Contains(UnitOfMeasureSelect.IsDecimal) ? q.IsDecimal : default(bool),
                Used = filter.Selects.Contains(UnitOfMeasureSelect.Used) ? q.Used : default(bool),
                ErpCode = filter.Selects.Contains(UnitOfMeasureSelect.ErpCode) ? q.ErpCode : default(string),
                Status = filter.Selects.Contains(UnitOfMeasureSelect.Status) && q.Status != null ? new Status
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
            return UnitOfMeasures;
        }

        public async Task<int> CountAll(UnitOfMeasureFilter filter)
        {
            IQueryable<UnitOfMeasureDAO> UnitOfMeasureDAOs = DataContext.UnitOfMeasure.AsNoTracking();
            UnitOfMeasureDAOs = await DynamicFilter(UnitOfMeasureDAOs, filter);
            return await UnitOfMeasureDAOs.CountAsync();
        }

        public async Task<int> Count(UnitOfMeasureFilter filter)
        {
            IQueryable<UnitOfMeasureDAO> UnitOfMeasureDAOs = DataContext.UnitOfMeasure.AsNoTracking();
            UnitOfMeasureDAOs = await DynamicFilter(UnitOfMeasureDAOs, filter);
            UnitOfMeasureDAOs = await OrFilter(UnitOfMeasureDAOs, filter);
            return await UnitOfMeasureDAOs.CountAsync();
        }

        public async Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter filter)
        {
            if (filter == null) return new List<UnitOfMeasure>();
            IQueryable<UnitOfMeasureDAO> UnitOfMeasureDAOs = DataContext.UnitOfMeasure.AsNoTracking();
            UnitOfMeasureDAOs = await DynamicFilter(UnitOfMeasureDAOs, filter);
            UnitOfMeasureDAOs = await OrFilter(UnitOfMeasureDAOs, filter);
            UnitOfMeasureDAOs = DynamicOrder(UnitOfMeasureDAOs, filter);
            List<UnitOfMeasure> UnitOfMeasures = await DynamicSelect(UnitOfMeasureDAOs, filter);
            return UnitOfMeasures;
        }

        public async Task<List<UnitOfMeasure>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<UnitOfMeasureDAO> query = DataContext.UnitOfMeasure.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<UnitOfMeasure> UnitOfMeasures = await query.AsNoTracking()
            .Select(x => new UnitOfMeasure()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StatusId = x.StatusId,
                IsDecimal = x.IsDecimal,
                Used = x.Used,
                ErpCode = x.ErpCode,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();


            return UnitOfMeasures;
        }

        public async Task<UnitOfMeasure> Get(long Id)
        {
            UnitOfMeasure UnitOfMeasure = await DataContext.UnitOfMeasure.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new UnitOfMeasure()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StatusId = x.StatusId,
                IsDecimal = x.IsDecimal,
                Used = x.Used,
                ErpCode = x.ErpCode,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (UnitOfMeasure == null)
                return null;

            return UnitOfMeasure;
        }

        public async Task<List<long>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures)
        {
            IdFilter IdFilter = new IdFilter { In = UnitOfMeasures.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<UnitOfMeasureDAO> UnitOfMeasureDAOs = new List<UnitOfMeasureDAO>();
            foreach (UnitOfMeasure UnitOfMeasure in UnitOfMeasures)
            {
                UnitOfMeasureDAO UnitOfMeasureDAO = new UnitOfMeasureDAO();
                UnitOfMeasureDAOs.Add(UnitOfMeasureDAO);
                UnitOfMeasureDAO.Id = UnitOfMeasure.Id;
                UnitOfMeasureDAO.Code = UnitOfMeasure.Code;
                UnitOfMeasureDAO.Name = UnitOfMeasure.Name;
                UnitOfMeasureDAO.Description = UnitOfMeasure.Description;
                UnitOfMeasureDAO.StatusId = UnitOfMeasure.StatusId;
                UnitOfMeasureDAO.IsDecimal = UnitOfMeasure.IsDecimal;
                UnitOfMeasureDAO.Used = UnitOfMeasure.Used;
                UnitOfMeasureDAO.ErpCode = UnitOfMeasure.ErpCode;
                UnitOfMeasureDAO.CreatedAt = UnitOfMeasure.CreatedAt;
                UnitOfMeasureDAO.UpdatedAt = UnitOfMeasure.UpdatedAt;
                UnitOfMeasureDAO.DeletedAt = UnitOfMeasure.DeletedAt;
                UnitOfMeasureDAO.RowId = UnitOfMeasure.RowId;
            }
            await DataContext.BulkMergeAsync(UnitOfMeasureDAOs);
            var Ids = UnitOfMeasureDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.UnitOfMeasure
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new UnitOfMeasureDAO { Used = true });
            return true;
        }
    }
}
