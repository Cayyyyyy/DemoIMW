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
    public interface IProductTypeRepository
    {
        Task<int> CountAll(ProductTypeFilter ProductTypeFilter);
        Task<int> Count(ProductTypeFilter ProductTypeFilter);
        Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter);
        Task<List<ProductType>> List(List<long> Ids);
        Task<ProductType> Get(long Id);
        Task<List<long>> BulkMerge(List<ProductType> ProductTypes);
    }
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly DataContext DataContext;
        public ProductTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<ProductTypeDAO>> DynamicFilter(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
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
                    (filter.SearchBy.Contains(ProductTypeSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(ProductTypeSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<ProductTypeDAO>> OrFilter(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProductTypeDAO> initQuery = query.Where(q => false);
            foreach (ProductTypeFilter ProductTypeFilter in filter.OrFilter)
            {
                IQueryable<ProductTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProductTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, ProductTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, ProductTypeFilter.Name);
                queryable = queryable.Where(q => q.Description, ProductTypeFilter.Description);
                queryable = queryable.Where(q => q.StatusId, ProductTypeFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<ProductTypeDAO> DynamicOrder(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProductTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProductTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProductTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProductTypeOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ProductTypeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ProductTypeOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProductTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProductTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProductTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProductTypeOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ProductTypeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ProductTypeOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ProductType>> DynamicSelect(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            List<ProductType> ProductTypes = await query.Select(q => new ProductType()
            {
                Id = filter.Selects.Contains(ProductTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProductTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProductTypeSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(ProductTypeSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(ProductTypeSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(ProductTypeSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(ProductTypeSelect.Status) && q.Status != null ? new Status
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
            return ProductTypes;
        }

        public async Task<int> CountAll(ProductTypeFilter filter)
        {
            IQueryable<ProductTypeDAO> ProductTypeDAOs = DataContext.ProductType.AsNoTracking();
            ProductTypeDAOs = await DynamicFilter(ProductTypeDAOs, filter);
            return await ProductTypeDAOs.CountAsync();
        }

        public async Task<int> Count(ProductTypeFilter filter)
        {
            IQueryable<ProductTypeDAO> ProductTypeDAOs = DataContext.ProductType.AsNoTracking();
            ProductTypeDAOs = await DynamicFilter(ProductTypeDAOs, filter);
            ProductTypeDAOs = await OrFilter(ProductTypeDAOs, filter);
            return await ProductTypeDAOs.CountAsync();
        }

        public async Task<List<ProductType>> List(ProductTypeFilter filter)
        {
            if (filter == null) return new List<ProductType>();
            IQueryable<ProductTypeDAO> ProductTypeDAOs = DataContext.ProductType.AsNoTracking();
            ProductTypeDAOs = await DynamicFilter(ProductTypeDAOs, filter);
            ProductTypeDAOs = await OrFilter(ProductTypeDAOs, filter);
            ProductTypeDAOs = DynamicOrder(ProductTypeDAOs, filter);
            List<ProductType> ProductTypes = await DynamicSelect(ProductTypeDAOs, filter);
            return ProductTypes;
        }

        public async Task<List<ProductType>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<ProductTypeDAO> query = DataContext.ProductType.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<ProductType> ProductTypes = await query.AsNoTracking()
            .Select(x => new ProductType()
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
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return ProductTypes;
        }

        public async Task<ProductType> Get(long Id)
        {
            ProductType ProductType = await DataContext.ProductType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ProductType()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
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

            if (ProductType == null)
                return null;

            return ProductType;
        }

        public async Task<List<long>> BulkMerge(List<ProductType> ProductTypes)
        {
            IdFilter IdFilter = new IdFilter { In = ProductTypes.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<ProductTypeDAO> ProductTypeDAOs = new List<ProductTypeDAO>();
            foreach (ProductType ProductType in ProductTypes)
            {
                ProductTypeDAO ProductTypeDAO = new ProductTypeDAO();
                ProductTypeDAOs.Add(ProductTypeDAO);
                ProductTypeDAO.Id = ProductType.Id;
                ProductTypeDAO.Code = ProductType.Code;
                ProductTypeDAO.Name = ProductType.Name;
                ProductTypeDAO.Description = ProductType.Description;
                ProductTypeDAO.StatusId = ProductType.StatusId;
                ProductTypeDAO.Used = ProductType.Used;
                ProductTypeDAO.CreatedAt = ProductType.CreatedAt;
                ProductTypeDAO.UpdatedAt = ProductType.UpdatedAt;
                ProductTypeDAO.DeletedAt = ProductType.DeletedAt;
                ProductTypeDAO.RowId = ProductType.RowId;
            }
            await DataContext.BulkMergeAsync(ProductTypeDAOs);
            var Ids = ProductTypeDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.ProductType
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new ProductTypeDAO { Used = true });
            return true;
        }
    }
}
