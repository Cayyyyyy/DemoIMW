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
    public interface IDistrictRepository
    {
        Task<int> CountAll(DistrictFilter DistrictFilter);
        Task<int> Count(DistrictFilter DistrictFilter);
        Task<List<District>> List(DistrictFilter DistrictFilter);
        Task<List<District>> List(List<long> Ids);
        Task<District> Get(long Id);
        Task<List<long>> BulkMerge(List<District> Districts);
    }
    public class DistrictRepository : IDistrictRepository
    {
        private readonly DataContext DataContext;
        public DistrictRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<DistrictDAO>> DynamicFilter(IQueryable<DistrictDAO> query, DistrictFilter filter)
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
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(DistrictSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(DistrictSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<DistrictDAO>> OrFilter(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (DistrictFilter DistrictFilter in filter.OrFilter)
            {
                IQueryable<DistrictDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DistrictFilter.Id);
                queryable = queryable.Where(q => q.Code, DistrictFilter.Code);
                queryable = queryable.Where(q => q.Name, DistrictFilter.Name);
                queryable = queryable.Where(q => q.Priority, DistrictFilter.Priority);
                queryable = queryable.Where(q => q.ProvinceId, DistrictFilter.ProvinceId);
                queryable = queryable.Where(q => q.StatusId, DistrictFilter.StatusId);
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

        private IQueryable<DistrictDAO> DynamicOrder(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            Dictionary<DistrictOrder, LambdaExpression> CustomOrder = new Dictionary<DistrictOrder, LambdaExpression>()
            {
                { DistrictOrder.Province, (DistrictDAO x) => x.Province.Name  },
                { DistrictOrder.Status, (DistrictDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<District>> DynamicSelect(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            List<District> Districts = await query.Select(q => new District()
            {
                Id = filter.Selects.Contains(DistrictSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(DistrictSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(DistrictSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(DistrictSelect.Priority) ? q.Priority : default(long?),
                ProvinceId = filter.Selects.Contains(DistrictSelect.Province) ? q.ProvinceId : default(long),
                StatusId = filter.Selects.Contains(DistrictSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(DistrictSelect.Used) ? q.Used : default(bool),
                Province = filter.Selects.Contains(DistrictSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                    Priority = q.Province.Priority,
                    StatusId = q.Province.StatusId,
                    Used = q.Province.Used,
                } : null,
                Status = filter.Selects.Contains(DistrictSelect.Status) && q.Status != null ? new Status
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
            return Districts;
        }

        public async Task<int> CountAll(DistrictFilter filter)
        {
            IQueryable<DistrictDAO> DistrictDAOs = DataContext.District.AsNoTracking();
            DistrictDAOs = await DynamicFilter(DistrictDAOs, filter);
            return await DistrictDAOs.CountAsync();
        }

        public async Task<int> Count(DistrictFilter filter)
        {
            IQueryable<DistrictDAO> DistrictDAOs = DataContext.District.AsNoTracking();
            DistrictDAOs = await DynamicFilter(DistrictDAOs, filter);
            DistrictDAOs = await OrFilter(DistrictDAOs, filter);
            return await DistrictDAOs.CountAsync();
        }

        public async Task<List<District>> List(DistrictFilter filter)
        {
            if (filter == null) return new List<District>();
            IQueryable<DistrictDAO> DistrictDAOs = DataContext.District.AsNoTracking();
            DistrictDAOs = await DynamicFilter(DistrictDAOs, filter);
            DistrictDAOs = await OrFilter(DistrictDAOs, filter);
            DistrictDAOs = DynamicOrder(DistrictDAOs, filter);
            List<District> Districts = await DynamicSelect(DistrictDAOs, filter);
            return Districts;
        }

        public async Task<List<District>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<DistrictDAO> query = DataContext.District.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<District> Districts = await query.AsNoTracking()
            .Select(x => new District()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                ProvinceId = x.ProvinceId,
                StatusId = x.StatusId,
                Used = x.Used,
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                    Used = x.Province.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();


            return Districts;
        }

        public async Task<District> Get(long Id)
        {
            District District = await DataContext.District.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new District()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                ProvinceId = x.ProvinceId,
                StatusId = x.StatusId,
                Used = x.Used,
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                    Used = x.Province.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (District == null)
                return null;

            return District;
        }

        public async Task<List<long>> BulkMerge(List<District> Districts)
        {
            IdFilter IdFilter = new IdFilter { In = Districts.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<DistrictDAO> DistrictDAOs = new List<DistrictDAO>();
            foreach (District District in Districts)
            {
                DistrictDAO DistrictDAO = new DistrictDAO();
                DistrictDAOs.Add(DistrictDAO);
                DistrictDAO.Id = District.Id;
                DistrictDAO.Code = District.Code;
                DistrictDAO.Name = District.Name;
                DistrictDAO.Priority = District.Priority;
                DistrictDAO.ProvinceId = District.ProvinceId;
                DistrictDAO.StatusId = District.StatusId;
                DistrictDAO.Used = District.Used;
                DistrictDAO.CreatedAt = District.CreatedAt;
                DistrictDAO.UpdatedAt = District.UpdatedAt;
                DistrictDAO.DeletedAt = District.DeletedAt;
                DistrictDAO.RowId = District.RowId;
            }
            await DataContext.BulkMergeAsync(DistrictDAOs);
            var Ids = DistrictDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.District
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new DistrictDAO { Used = true });
            return true;
        }
    }
}
