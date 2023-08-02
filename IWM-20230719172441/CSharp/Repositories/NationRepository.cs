using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Models;
using IWM.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace IWM.Repositories
{
    public interface INationRepository
    {
        Task<int> CountAll(NationFilter NationFilter);
        Task<int> Count(NationFilter NationFilter);
        Task<List<Nation>> List(NationFilter NationFilter);
        Task<List<Nation>> List(List<long> Ids);
        Task<Nation> Get(long Id);
        Task<List<long>> BulkMerge(List<Nation> Nations);
    }
    public class NationRepository : INationRepository
    {
        private readonly DataContext DataContext;
        public NationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<NationDAO>> DynamicFilter(IQueryable<NationDAO> query, NationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(NationSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(NationSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<NationDAO>> OrFilter(IQueryable<NationDAO> query, NationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<NationDAO> initQuery = query.Where(q => false);
            foreach (NationFilter NationFilter in filter.OrFilter)
            {
                IQueryable<NationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, NationFilter.Id);
                queryable = queryable.Where(q => q.Code, NationFilter.Code);
                queryable = queryable.Where(q => q.Name, NationFilter.Name);
                queryable = queryable.Where(q => q.Priority, NationFilter.Priority);
                queryable = queryable.Where(q => q.StatusId, NationFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<NationDAO> DynamicOrder(IQueryable<NationDAO> query, NationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case NationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case NationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case NationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case NationOrder.Priority:
                            query = query.OrderBy(q => q.Priority);
                            break;
                        case NationOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case NationOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case NationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case NationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case NationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case NationOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority);
                            break;
                        case NationOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case NationOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Nation>> DynamicSelect(IQueryable<NationDAO> query, NationFilter filter)
        {
            List<Nation> Nations = await query.Select(q => new Nation()
            {
                Id = filter.Selects.Contains(NationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(NationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(NationSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(NationSelect.Priority) ? q.Priority : default(long?),
                StatusId = filter.Selects.Contains(NationSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(NationSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(NationSelect.Status) && q.Status != null ? new Status
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
            return Nations;
        }

        public async Task<int> CountAll(NationFilter filter)
        {
            IQueryable<NationDAO> NationDAOs = DataContext.Nation.AsNoTracking();
            NationDAOs = await DynamicFilter(NationDAOs, filter);
            return await NationDAOs.CountAsync();
        }

        public async Task<int> Count(NationFilter filter)
        {
            IQueryable<NationDAO> NationDAOs = DataContext.Nation.AsNoTracking();
            NationDAOs = await DynamicFilter(NationDAOs, filter);
            NationDAOs = await OrFilter(NationDAOs, filter);
            return await NationDAOs.CountAsync();
        }

        public async Task<List<Nation>> List(NationFilter filter)
        {
            if (filter == null) return new List<Nation>();
            IQueryable<NationDAO> NationDAOs = DataContext.Nation.AsNoTracking();
            NationDAOs = await DynamicFilter(NationDAOs, filter);
            NationDAOs = await OrFilter(NationDAOs, filter);
            NationDAOs = DynamicOrder(NationDAOs, filter);
            List<Nation> Nations = await DynamicSelect(NationDAOs, filter);
            return Nations;
        }

        public async Task<List<Nation>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<NationDAO> query = DataContext.Nation.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Nation> Nations = await query.AsNoTracking()
            .Select(x => new Nation()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
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
            

            return Nations;
        }

        public async Task<Nation> Get(long Id)
        {
            Nation Nation = await DataContext.Nation.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Nation()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
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

            if (Nation == null)
                return null;

            return Nation;
        }

        public async Task<List<long>> BulkMerge(List<Nation> Nations)
        {
            IdFilter IdFilter = new IdFilter { In = Nations.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<NationDAO> NationDAOs = new List<NationDAO>();
            foreach (Nation Nation in Nations)
            {
                NationDAO NationDAO = new NationDAO();
                NationDAOs.Add(NationDAO);
                NationDAO.Id = Nation.Id;
                NationDAO.Code = Nation.Code;
                NationDAO.Name = Nation.Name;
                NationDAO.Priority = Nation.Priority;
                NationDAO.StatusId = Nation.StatusId;
                NationDAO.Used = Nation.Used;
                NationDAO.CreatedAt = Nation.CreatedAt;
                NationDAO.UpdatedAt = Nation.UpdatedAt;
                NationDAO.DeletedAt = Nation.DeletedAt;
                NationDAO.RowId = Nation.RowId;
            }
            await DataContext.BulkMergeAsync(NationDAOs);
            var Ids = NationDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Nation
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new NationDAO { Used = true });
            return true;
        }
    }
}
