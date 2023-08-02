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
    public interface IStatusRepository
    {
        Task<int> CountAll(StatusFilter StatusFilter);
        Task<int> Count(StatusFilter StatusFilter);
        Task<List<Status>> List(StatusFilter StatusFilter);
        Task<List<Status>> List(List<long> Ids);
        Task<bool> BulkMerge(List<Status> Statuses);
    }
    public class StatusRepository : IStatusRepository
    {
        private readonly DataContext DataContext;
        public StatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<StatusDAO>> DynamicFilter(IQueryable<StatusDAO> query, StatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Color, filter.Color);

            return query;
        }

        private async Task<IQueryable<StatusDAO>> OrFilter(IQueryable<StatusDAO> query, StatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StatusDAO> initQuery = query.Where(q => false);
            foreach (StatusFilter StatusFilter in filter.OrFilter)
            {
                IQueryable<StatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StatusFilter.Id);
                queryable = queryable.Where(q => q.Code, StatusFilter.Code);
                queryable = queryable.Where(q => q.Name, StatusFilter.Name);
                queryable = queryable.Where(q => q.Color, StatusFilter.Color);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<StatusDAO> DynamicOrder(IQueryable<StatusDAO> query, StatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case StatusOrder.Color:
                            query = query.OrderBy(q => q.Color);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case StatusOrder.Color:
                            query = query.OrderByDescending(q => q.Color);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Status>> DynamicSelect(IQueryable<StatusDAO> query, StatusFilter filter)
        {
            List<Status> Statuses = await query.Select(q => new Status()
            {
                Id = filter.Selects.Contains(StatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StatusSelect.Name) ? q.Name : default(string),
                Color = filter.Selects.Contains(StatusSelect.Color) ? q.Color : default(string),
            }).ToListAsync();
            return Statuses;
        }

        public async Task<int> CountAll(StatusFilter filter)
        {
            IQueryable<StatusDAO> StatusDAOs = DataContext.Status.AsNoTracking();
            StatusDAOs = await DynamicFilter(StatusDAOs, filter);
            return await StatusDAOs.CountAsync();
        }

        public async Task<int> Count(StatusFilter filter)
        {
            IQueryable<StatusDAO> StatusDAOs = DataContext.Status.AsNoTracking();
            StatusDAOs = await DynamicFilter(StatusDAOs, filter);
            StatusDAOs = await OrFilter(StatusDAOs, filter);
            return await StatusDAOs.CountAsync();
        }

        public async Task<List<Status>> List(StatusFilter filter)
        {
            if (filter == null) return new List<Status>();
            IQueryable<StatusDAO> StatusDAOs = DataContext.Status.AsNoTracking();
            StatusDAOs = await DynamicFilter(StatusDAOs, filter);
            StatusDAOs = await OrFilter(StatusDAOs, filter);
            StatusDAOs = DynamicOrder(StatusDAOs, filter);
            List<Status> Statuses = await DynamicSelect(StatusDAOs, filter);
            return Statuses;
        }

        public async Task<List<Status>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<StatusDAO> query = DataContext.Status.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Status> Statuses = await query.AsNoTracking()
            .Select(x => new Status()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Color = x.Color,
            }).ToListAsync();
            

            return Statuses;
        }

        public async Task<bool> BulkMerge(List<Status> Statuses)
        {
            List<StatusDAO> StatusDAOs = new List<StatusDAO>();
            foreach (var Status in Statuses)
            {
                StatusDAO StatusDAO = new StatusDAO();
                StatusDAO.Id = Status.Id;
                StatusDAO.Code = Status.Code;
                StatusDAO.Name = Status.Name;
                StatusDAO.Color = Status.Color;
                StatusDAOs.Add(StatusDAO);
            }
            await DataContext.Status.BulkMergeAsync(StatusDAOs);
            return true;
        }
    }
}
