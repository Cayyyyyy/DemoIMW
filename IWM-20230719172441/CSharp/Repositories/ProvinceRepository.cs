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
    public interface IProvinceRepository
    {
        Task<int> CountAll(ProvinceFilter ProvinceFilter);
        Task<int> Count(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(List<long> Ids);
        Task<Province> Get(long Id);
        Task<List<long>> BulkMerge(List<Province> Provinces);
    }
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly DataContext DataContext;
        public ProvinceRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<ProvinceDAO>> DynamicFilter(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
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
                    (filter.SearchBy.Contains(ProvinceSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(ProvinceSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<ProvinceDAO>> OrFilter(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProvinceDAO> initQuery = query.Where(q => false);
            foreach (ProvinceFilter ProvinceFilter in filter.OrFilter)
            {
                IQueryable<ProvinceDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProvinceFilter.Id);
                queryable = queryable.Where(q => q.Code, ProvinceFilter.Code);
                queryable = queryable.Where(q => q.Name, ProvinceFilter.Name);
                queryable = queryable.Where(q => q.Priority, ProvinceFilter.Priority);
                queryable = queryable.Where(q => q.StatusId, ProvinceFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<ProvinceDAO> DynamicOrder(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            query = query.OrderBy(q => q.Priority);
                            break;
                        case ProvinceOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ProvinceOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority);
                            break;
                        case ProvinceOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ProvinceOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Province>> DynamicSelect(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            List<Province> Provinces = await query.Select(q => new Province()
            {
                Id = filter.Selects.Contains(ProvinceSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProvinceSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProvinceSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(ProvinceSelect.Priority) ? q.Priority : default(long?),
                StatusId = filter.Selects.Contains(ProvinceSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(ProvinceSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(ProvinceSelect.Status) && q.Status != null ? new Status
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
            return Provinces;
        }

        public async Task<int> CountAll(ProvinceFilter filter)
        {
            IQueryable<ProvinceDAO> ProvinceDAOs = DataContext.Province.AsNoTracking();
            ProvinceDAOs = await DynamicFilter(ProvinceDAOs, filter);
            return await ProvinceDAOs.CountAsync();
        }

        public async Task<int> Count(ProvinceFilter filter)
        {
            IQueryable<ProvinceDAO> ProvinceDAOs = DataContext.Province.AsNoTracking();
            ProvinceDAOs = await DynamicFilter(ProvinceDAOs, filter);
            ProvinceDAOs = await OrFilter(ProvinceDAOs, filter);
            return await ProvinceDAOs.CountAsync();
        }

        public async Task<List<Province>> List(ProvinceFilter filter)
        {
            if (filter == null) return new List<Province>();
            IQueryable<ProvinceDAO> ProvinceDAOs = DataContext.Province.AsNoTracking();
            ProvinceDAOs = await DynamicFilter(ProvinceDAOs, filter);
            ProvinceDAOs = await OrFilter(ProvinceDAOs, filter);
            ProvinceDAOs = DynamicOrder(ProvinceDAOs, filter);
            List<Province> Provinces = await DynamicSelect(ProvinceDAOs, filter);
            return Provinces;
        }

        public async Task<List<Province>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<ProvinceDAO> query = DataContext.Province.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Province> Provinces = await query.AsNoTracking()
            .Select(x => new Province()
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
            

            return Provinces;
        }

        public async Task<Province> Get(long Id)
        {
            Province Province = await DataContext.Province.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Province()
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

            if (Province == null)
                return null;

            return Province;
        }

        public async Task<List<long>> BulkMerge(List<Province> Provinces)
        {
            IdFilter IdFilter = new IdFilter { In = Provinces.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<ProvinceDAO> ProvinceDAOs = new List<ProvinceDAO>();
            foreach (Province Province in Provinces)
            {
                ProvinceDAO ProvinceDAO = new ProvinceDAO();
                ProvinceDAOs.Add(ProvinceDAO);
                ProvinceDAO.Id = Province.Id;
                ProvinceDAO.Code = Province.Code;
                ProvinceDAO.Name = Province.Name;
                ProvinceDAO.Priority = Province.Priority;
                ProvinceDAO.StatusId = Province.StatusId;
                ProvinceDAO.Used = Province.Used;
                ProvinceDAO.CreatedAt = Province.CreatedAt;
                ProvinceDAO.UpdatedAt = Province.UpdatedAt;
                ProvinceDAO.DeletedAt = Province.DeletedAt;
                ProvinceDAO.RowId = Province.RowId;
            }
            await DataContext.BulkMergeAsync(ProvinceDAOs);
            var Ids = ProvinceDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Province
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new ProvinceDAO { Used = true });
            return true;
        }
    }
}
