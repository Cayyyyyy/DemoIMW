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
    public interface IWorkerGroupRepository
    {
        Task<int> CountAll(WorkerGroupFilter WorkerGroupFilter);
        Task<int> Count(WorkerGroupFilter WorkerGroupFilter);
        Task<List<WorkerGroup>> List(WorkerGroupFilter WorkerGroupFilter);
        Task<List<WorkerGroup>> List(List<long> Ids);
        Task<WorkerGroup> Get(long Id);
        Task<bool> Create(WorkerGroup WorkerGroup);
        Task<bool> Update(WorkerGroup WorkerGroup);
        Task<bool> Delete(WorkerGroup WorkerGroup);
        Task<List<long>> BulkMerge(List<WorkerGroup> WorkerGroups);
        Task<bool> BulkDelete(List<WorkerGroup> WorkerGroups);
    }
    public class WorkerGroupRepository : IWorkerGroupRepository
    {
        private readonly DataContext DataContext;
        public WorkerGroupRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<WorkerGroupDAO>> DynamicFilter(IQueryable<WorkerGroupDAO> query, WorkerGroupFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(WorkerGroupSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(WorkerGroupSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<WorkerGroupDAO>> OrFilter(IQueryable<WorkerGroupDAO> query, WorkerGroupFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkerGroupDAO> initQuery = query.Where(q => false);
            foreach (WorkerGroupFilter WorkerGroupFilter in filter.OrFilter)
            {
                IQueryable<WorkerGroupDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkerGroupFilter.Id);
                queryable = queryable.Where(q => q.Code, WorkerGroupFilter.Code);
                queryable = queryable.Where(q => q.Name, WorkerGroupFilter.Name);
                queryable = queryable.Where(q => q.StatusId, WorkerGroupFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<WorkerGroupDAO> DynamicOrder(IQueryable<WorkerGroupDAO> query, WorkerGroupFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkerGroupOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkerGroupOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WorkerGroupOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WorkerGroupOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkerGroupOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkerGroupOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WorkerGroupOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WorkerGroupOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<WorkerGroup>> DynamicSelect(IQueryable<WorkerGroupDAO> query, WorkerGroupFilter filter)
        {
            List<WorkerGroup> WorkerGroups = await query.Select(q => new WorkerGroup()
            {
                Id = filter.Selects.Contains(WorkerGroupSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WorkerGroupSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WorkerGroupSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(WorkerGroupSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(WorkerGroupSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                    Color = q.Status.Color,
                } : null,
            }).ToListAsync();
            return WorkerGroups;
        }

        public async Task<int> CountAll(WorkerGroupFilter filter)
        {
            IQueryable<WorkerGroupDAO> WorkerGroupDAOs = DataContext.WorkerGroup.AsNoTracking();
            WorkerGroupDAOs = await DynamicFilter(WorkerGroupDAOs, filter);
            return await WorkerGroupDAOs.CountAsync();
        }

        public async Task<int> Count(WorkerGroupFilter filter)
        {
            IQueryable<WorkerGroupDAO> WorkerGroupDAOs = DataContext.WorkerGroup.AsNoTracking();
            WorkerGroupDAOs = await DynamicFilter(WorkerGroupDAOs, filter);
            WorkerGroupDAOs = await OrFilter(WorkerGroupDAOs, filter);
            return await WorkerGroupDAOs.CountAsync();
        }

        public async Task<List<WorkerGroup>> List(WorkerGroupFilter filter)
        {
            if (filter == null) return new List<WorkerGroup>();
            IQueryable<WorkerGroupDAO> WorkerGroupDAOs = DataContext.WorkerGroup.AsNoTracking();
            WorkerGroupDAOs = await DynamicFilter(WorkerGroupDAOs, filter);
            WorkerGroupDAOs = await OrFilter(WorkerGroupDAOs, filter);
            WorkerGroupDAOs = DynamicOrder(WorkerGroupDAOs, filter);
            List<WorkerGroup> WorkerGroups = await DynamicSelect(WorkerGroupDAOs, filter);
            return WorkerGroups;
        }

        public async Task<List<WorkerGroup>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<WorkerGroupDAO> query = DataContext.WorkerGroup.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<WorkerGroup> WorkerGroups = await query.AsNoTracking()
            .Select(x => new WorkerGroup()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return WorkerGroups;
        }

        public async Task<WorkerGroup> Get(long Id)
        {
            WorkerGroup WorkerGroup = await DataContext.WorkerGroup.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new WorkerGroup()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (WorkerGroup == null)
                return null;

            return WorkerGroup;
        }
        public async Task<bool> Create(WorkerGroup WorkerGroup)
        {
            WorkerGroupDAO WorkerGroupDAO = new WorkerGroupDAO();
            WorkerGroupDAO.Id = WorkerGroup.Id;
            WorkerGroupDAO.Code = WorkerGroup.Code;
            WorkerGroupDAO.Name = WorkerGroup.Name;
            WorkerGroupDAO.StatusId = WorkerGroup.StatusId;
            DataContext.WorkerGroup.Add(WorkerGroupDAO);
            await DataContext.SaveChangesAsync();
            WorkerGroup.Id = WorkerGroupDAO.Id;
            await SaveReference(WorkerGroup);
            return true;
        }

        public async Task<bool> Update(WorkerGroup WorkerGroup)
        {
            WorkerGroupDAO WorkerGroupDAO = DataContext.WorkerGroup
                .Where(x => x.Id == WorkerGroup.Id)
                .FirstOrDefault();
            if (WorkerGroupDAO == null)
                return false;
            WorkerGroupDAO.Id = WorkerGroup.Id;
            WorkerGroupDAO.Code = WorkerGroup.Code;
            WorkerGroupDAO.Name = WorkerGroup.Name;
            WorkerGroupDAO.StatusId = WorkerGroup.StatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(WorkerGroup);
            return true;
        }

        public async Task<bool> Delete(WorkerGroup WorkerGroup)
        {
            await DataContext.WorkerGroup
                .Where(x => x.Id == WorkerGroup.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<List<long>> BulkMerge(List<WorkerGroup> WorkerGroups)
        {
            IdFilter IdFilter = new IdFilter { In = WorkerGroups.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<WorkerGroupDAO> WorkerGroupDAOs = new List<WorkerGroupDAO>();
            foreach (WorkerGroup WorkerGroup in WorkerGroups)
            {
                WorkerGroupDAO WorkerGroupDAO = new WorkerGroupDAO();
                WorkerGroupDAOs.Add(WorkerGroupDAO);
                WorkerGroupDAO.Id = WorkerGroup.Id;
                WorkerGroupDAO.Code = WorkerGroup.Code;
                WorkerGroupDAO.Name = WorkerGroup.Name;
                WorkerGroupDAO.StatusId = WorkerGroup.StatusId;
            }
            await DataContext.BulkMergeAsync(WorkerGroupDAOs);
            var Ids = WorkerGroupDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        
        public async Task<bool> BulkDelete(List<WorkerGroup> WorkerGroups)
        {
            List<long> Ids = WorkerGroups.Select(x => x.Id).ToList();
            await DataContext.WorkerGroup
                .WhereBulkContains(Ids, x => x.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(WorkerGroup WorkerGroup)
        {
        }

    }
}
