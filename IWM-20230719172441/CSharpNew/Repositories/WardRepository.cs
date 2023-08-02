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
    public interface IWardRepository
    {
        Task<int> CountAll(WardFilter WardFilter);
        Task<int> Count(WardFilter WardFilter);
        Task<List<Ward>> List(WardFilter WardFilter);
        Task<List<Ward>> List(List<long> Ids);
        Task<Ward> Get(long Id);
        Task<List<long>> BulkMerge(List<Ward> Wards);
    }
    public class WardRepository : IWardRepository
    {
        private readonly DataContext DataContext;
        public WardRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<WardDAO>> DynamicFilter(IQueryable<WardDAO> query, WardFilter filter)
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
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(WardSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(WardSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<WardDAO>> OrFilter(IQueryable<WardDAO> query, WardFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (WardFilter WardFilter in filter.OrFilter)
            {
                IQueryable<WardDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WardFilter.Id);
                queryable = queryable.Where(q => q.Code, WardFilter.Code);
                queryable = queryable.Where(q => q.Name, WardFilter.Name);
                queryable = queryable.Where(q => q.Priority, WardFilter.Priority);
                queryable = queryable.Where(q => q.DistrictId, WardFilter.DistrictId);
                queryable = queryable.Where(q => q.StatusId, WardFilter.StatusId);
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

        private IQueryable<WardDAO> DynamicOrder(IQueryable<WardDAO> query, WardFilter filter)
        {
            Dictionary<WardOrder, LambdaExpression> CustomOrder = new Dictionary<WardOrder, LambdaExpression>()
            {
                { WardOrder.District, (WardDAO x) => x.District.Name  },
                { WardOrder.Status, (WardDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<Ward>> DynamicSelect(IQueryable<WardDAO> query, WardFilter filter)
        {
            List<Ward> Wards = await query.Select(q => new Ward()
            {
                Id = filter.Selects.Contains(WardSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WardSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WardSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(WardSelect.Priority) ? q.Priority : default(long?),
                DistrictId = filter.Selects.Contains(WardSelect.District) ? q.DistrictId : default(long),
                StatusId = filter.Selects.Contains(WardSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(WardSelect.Used) ? q.Used : default(bool),
                District = filter.Selects.Contains(WardSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                    Priority = q.District.Priority,
                    ProvinceId = q.District.ProvinceId,
                    StatusId = q.District.StatusId,
                    Used = q.District.Used,
                } : null,
                Status = filter.Selects.Contains(WardSelect.Status) && q.Status != null ? new Status
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
            return Wards;
        }

        public async Task<int> CountAll(WardFilter filter)
        {
            IQueryable<WardDAO> WardDAOs = DataContext.Ward.AsNoTracking();
            WardDAOs = await DynamicFilter(WardDAOs, filter);
            return await WardDAOs.CountAsync();
        }

        public async Task<int> Count(WardFilter filter)
        {
            IQueryable<WardDAO> WardDAOs = DataContext.Ward.AsNoTracking();
            WardDAOs = await DynamicFilter(WardDAOs, filter);
            WardDAOs = await OrFilter(WardDAOs, filter);
            return await WardDAOs.CountAsync();
        }

        public async Task<List<Ward>> List(WardFilter filter)
        {
            if (filter == null) return new List<Ward>();
            IQueryable<WardDAO> WardDAOs = DataContext.Ward.AsNoTracking();
            WardDAOs = await DynamicFilter(WardDAOs, filter);
            WardDAOs = await OrFilter(WardDAOs, filter);
            WardDAOs = DynamicOrder(WardDAOs, filter);
            List<Ward> Wards = await DynamicSelect(WardDAOs, filter);
            return Wards;
        }

        public async Task<List<Ward>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<WardDAO> query = DataContext.Ward.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Ward> Wards = await query.AsNoTracking()
            .Select(x => new Ward()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                DistrictId = x.DistrictId,
                StatusId = x.StatusId,
                Used = x.Used,
                District = x.District == null ? null : new District
                {
                    Id = x.District.Id,
                    Code = x.District.Code,
                    Name = x.District.Name,
                    Priority = x.District.Priority,
                    ProvinceId = x.District.ProvinceId,
                    StatusId = x.District.StatusId,
                    Used = x.District.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();


            return Wards;
        }

        public async Task<Ward> Get(long Id)
        {
            Ward Ward = await DataContext.Ward.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Ward()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                DistrictId = x.DistrictId,
                StatusId = x.StatusId,
                Used = x.Used,
                District = x.District == null ? null : new District
                {
                    Id = x.District.Id,
                    Code = x.District.Code,
                    Name = x.District.Name,
                    Priority = x.District.Priority,
                    ProvinceId = x.District.ProvinceId,
                    StatusId = x.District.StatusId,
                    Used = x.District.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (Ward == null)
                return null;

            return Ward;
        }

        public async Task<List<long>> BulkMerge(List<Ward> Wards)
        {
            IdFilter IdFilter = new IdFilter { In = Wards.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<WardDAO> WardDAOs = new List<WardDAO>();
            foreach (Ward Ward in Wards)
            {
                WardDAO WardDAO = new WardDAO();
                WardDAOs.Add(WardDAO);
                WardDAO.Id = Ward.Id;
                WardDAO.Code = Ward.Code;
                WardDAO.Name = Ward.Name;
                WardDAO.Priority = Ward.Priority;
                WardDAO.DistrictId = Ward.DistrictId;
                WardDAO.StatusId = Ward.StatusId;
                WardDAO.Used = Ward.Used;
                WardDAO.CreatedAt = Ward.CreatedAt;
                WardDAO.UpdatedAt = Ward.UpdatedAt;
                WardDAO.DeletedAt = Ward.DeletedAt;
                WardDAO.RowId = Ward.RowId;
            }
            await DataContext.BulkMergeAsync(WardDAOs);
            var Ids = WardDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Ward
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new WardDAO { Used = true });
            return true;
        }
    }
}
