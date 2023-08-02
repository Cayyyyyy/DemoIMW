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
    public interface IBrandRepository
    {
        Task<int> CountAll(BrandFilter BrandFilter);
        Task<int> Count(BrandFilter BrandFilter);
        Task<List<Brand>> List(BrandFilter BrandFilter);
        Task<List<Brand>> List(List<long> Ids);
        Task<Brand> Get(long Id);
        Task<List<long>> BulkMerge(List<Brand> Brands);
    }
    public class BrandRepository : IBrandRepository
    {
        private readonly DataContext DataContext;
        public BrandRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<BrandDAO>> DynamicFilter(IQueryable<BrandDAO> query, BrandFilter filter)
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
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(BrandSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(BrandSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<BrandDAO>> OrFilter(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<BrandDAO> initQuery = query.Where(q => false);
            foreach (BrandFilter BrandFilter in filter.OrFilter)
            {
                IQueryable<BrandDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, BrandFilter.Id);
                queryable = queryable.Where(q => q.Code, BrandFilter.Code);
                queryable = queryable.Where(q => q.Name, BrandFilter.Name);
                queryable = queryable.Where(q => q.Description, BrandFilter.Description);
                queryable = queryable.Where(q => q.StatusId, BrandFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<BrandDAO> DynamicOrder(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case BrandOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case BrandOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case BrandOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case BrandOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case BrandOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case BrandOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case BrandOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case BrandOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case BrandOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case BrandOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case BrandOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case BrandOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Brand>> DynamicSelect(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            List<Brand> Brands = await query.Select(q => new Brand()
            {
                Id = filter.Selects.Contains(BrandSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(BrandSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(BrandSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(BrandSelect.Status) ? q.StatusId : default(long),
                Description = filter.Selects.Contains(BrandSelect.Description) ? q.Description : default(string),
                Used = filter.Selects.Contains(BrandSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(BrandSelect.Status) && q.Status != null ? new Status
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
            return Brands;
        }

        public async Task<int> CountAll(BrandFilter filter)
        {
            IQueryable<BrandDAO> BrandDAOs = DataContext.Brand.AsNoTracking();
            BrandDAOs = await DynamicFilter(BrandDAOs, filter);
            return await BrandDAOs.CountAsync();
        }

        public async Task<int> Count(BrandFilter filter)
        {
            IQueryable<BrandDAO> BrandDAOs = DataContext.Brand.AsNoTracking();
            BrandDAOs = await DynamicFilter(BrandDAOs, filter);
            BrandDAOs = await OrFilter(BrandDAOs, filter);
            return await BrandDAOs.CountAsync();
        }

        public async Task<List<Brand>> List(BrandFilter filter)
        {
            if (filter == null) return new List<Brand>();
            IQueryable<BrandDAO> BrandDAOs = DataContext.Brand.AsNoTracking();
            BrandDAOs = await DynamicFilter(BrandDAOs, filter);
            BrandDAOs = await OrFilter(BrandDAOs, filter);
            BrandDAOs = DynamicOrder(BrandDAOs, filter);
            List<Brand> Brands = await DynamicSelect(BrandDAOs, filter);
            return Brands;
        }

        public async Task<List<Brand>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<BrandDAO> query = DataContext.Brand.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Brand> Brands = await query.AsNoTracking()
            .Select(x => new Brand()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Description = x.Description,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return Brands;
        }

        public async Task<Brand> Get(long Id)
        {
            Brand Brand = await DataContext.Brand.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Brand()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Description = x.Description,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (Brand == null)
                return null;

            return Brand;
        }

        public async Task<List<long>> BulkMerge(List<Brand> Brands)
        {
            IdFilter IdFilter = new IdFilter { In = Brands.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<BrandDAO> BrandDAOs = new List<BrandDAO>();
            foreach (Brand Brand in Brands)
            {
                BrandDAO BrandDAO = new BrandDAO();
                BrandDAOs.Add(BrandDAO);
                BrandDAO.Id = Brand.Id;
                BrandDAO.Code = Brand.Code;
                BrandDAO.Name = Brand.Name;
                BrandDAO.StatusId = Brand.StatusId;
                BrandDAO.Description = Brand.Description;
                BrandDAO.Used = Brand.Used;
                BrandDAO.CreatedAt = Brand.CreatedAt;
                BrandDAO.UpdatedAt = Brand.UpdatedAt;
                BrandDAO.DeletedAt = Brand.DeletedAt;
                BrandDAO.RowId = Brand.RowId;
            }
            await DataContext.BulkMergeAsync(BrandDAOs);
            var Ids = BrandDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Brand
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new BrandDAO { Used = true });
            return true;
        }
    }
}
