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
    public interface IProductRepository
    {
        Task<int> CountAll(ProductFilter ProductFilter);
        Task<int> Count(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<List<Product>> List(List<long> Ids);
        Task<Product> Get(long Id);
        Task<List<long>> BulkMerge(List<Product> Products);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext DataContext;
        public ProductRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<ProductDAO>> DynamicFilter(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.SupplierCode, filter.SupplierCode);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.ScanCode, filter.ScanCode);
            query = query.Where(q => q.ERPCode, filter.ERPCode);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.RetailPrice, filter.RetailPrice);
            query = query.Where(q => q.OtherName, filter.OtherName);
            query = query.Where(q => q.TechnicalName, filter.TechnicalName);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.IsPurchasable, filter.IsPurchasable);
            query = query.Where(q => q.IsSellable, filter.IsSellable);
            query = query.Where(q => q.IsNew, filter.IsNew);
            query = query.Where(q => q.IsCopyRightedProduct, filter.IsCopyRightedProduct);
            query = query.Where(q => q.IsBrandProduct, filter.IsBrandProduct);
            query = query.Where(q => q.BrandId, filter.BrandId);
            if (filter.CategoryId != null && filter.CategoryId.HasValue)
            {
                if (filter.CategoryId.Equal != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Category.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.CategoryId.NotEqual != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Category.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.CategoryId.In != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.In.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.CategoryId, IdFilter);
                }
                if (filter.CategoryId.NotIn != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.NotIn.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.CategoryId, IdFilter);
                }
            }
            query = query.Where(q => q.ProductTypeId, filter.ProductTypeId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.TaxTypeId, filter.TaxTypeId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.UnitOfMeasureGroupingId, filter.UnitOfMeasureGroupingId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(ProductSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(ProductSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<ProductDAO>> OrFilter(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProductDAO> initQuery = query.Where(q => false);
            foreach (ProductFilter ProductFilter in filter.OrFilter)
            {
                IQueryable<ProductDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProductFilter.Id);
                queryable = queryable.Where(q => q.Code, ProductFilter.Code);
                queryable = queryable.Where(q => q.SupplierCode, ProductFilter.SupplierCode);
                queryable = queryable.Where(q => q.Name, ProductFilter.Name);
                queryable = queryable.Where(q => q.Description, ProductFilter.Description);
                queryable = queryable.Where(q => q.ScanCode, ProductFilter.ScanCode);
                queryable = queryable.Where(q => q.ERPCode, ProductFilter.ERPCode);
                queryable = queryable.Where(q => q.SalePrice, ProductFilter.SalePrice);
                queryable = queryable.Where(q => q.RetailPrice, ProductFilter.RetailPrice);
                queryable = queryable.Where(q => q.OtherName, ProductFilter.OtherName);
                queryable = queryable.Where(q => q.TechnicalName, ProductFilter.TechnicalName);
                queryable = queryable.Where(q => q.Note, ProductFilter.Note);
                queryable = queryable.Where(q => q.IsPurchasable, ProductFilter.IsPurchasable);
                queryable = queryable.Where(q => q.IsSellable, ProductFilter.IsSellable);
                queryable = queryable.Where(q => q.IsNew, ProductFilter.IsNew);
                queryable = queryable.Where(q => q.IsCopyRightedProduct, ProductFilter.IsCopyRightedProduct);
                queryable = queryable.Where(q => q.IsBrandProduct, ProductFilter.IsBrandProduct);
                queryable = queryable.Where(q => q.BrandId, ProductFilter.BrandId);
                if (ProductFilter.CategoryId != null && ProductFilter.CategoryId.HasValue)
                {
                    if (ProductFilter.CategoryId.Equal != null)
                    {
                        CategoryDAO CategoryDAO = DataContext.Category
                            .Where(o => o.Id == ProductFilter.CategoryId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Category.Path.StartsWith(CategoryDAO.Path));
                    }
                    if (ProductFilter.CategoryId.NotEqual != null)
                    {
                        CategoryDAO CategoryDAO = DataContext.Category
                            .Where(o => o.Id == ProductFilter.CategoryId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Category.Path.StartsWith(CategoryDAO.Path));
                    }
                    if (ProductFilter.CategoryId.In != null)
                    {
                        List<CategoryDAO> CategoryDAOs = DataContext.Category
                            .Where(o => o.DeletedAt == null).ToList();
                        List<CategoryDAO> Parents = CategoryDAOs.Where(o => ProductFilter.CategoryId.In.Contains(o.Id)).ToList();
                        List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.CategoryId, IdFilter);
                    }
                    if (ProductFilter.CategoryId.NotIn != null)
                    {
                        List<CategoryDAO> CategoryDAOs = DataContext.Category
                            .Where(o => o.DeletedAt == null).ToList();
                        List<CategoryDAO> Parents = CategoryDAOs.Where(o => ProductFilter.CategoryId.NotIn.Contains(o.Id)).ToList();
                        List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.CategoryId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.ProductTypeId, ProductFilter.ProductTypeId);
                queryable = queryable.Where(q => q.StatusId, ProductFilter.StatusId);
                queryable = queryable.Where(q => q.TaxTypeId, ProductFilter.TaxTypeId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, ProductFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.UnitOfMeasureGroupingId, ProductFilter.UnitOfMeasureGroupingId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<ProductDAO> DynamicOrder(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProductOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProductOrder.SupplierCode:
                            query = query.OrderBy(q => q.SupplierCode);
                            break;
                        case ProductOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            query = query.OrderBy(q => q.ScanCode);
                            break;
                        case ProductOrder.ERPCode:
                            query = query.OrderBy(q => q.ERPCode);
                            break;
                        case ProductOrder.Category:
                            query = query.OrderBy(q => q.CategoryId);
                            break;
                        case ProductOrder.ProductType:
                            query = query.OrderBy(q => q.ProductTypeId);
                            break;
                        case ProductOrder.Brand:
                            query = query.OrderBy(q => q.BrandId);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case ProductOrder.CodeGeneratorRule:
                            query = query.OrderBy(q => q.CodeGeneratorRuleId);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            query = query.OrderBy(q => q.UnitOfMeasureGroupingId);
                            break;
                        case ProductOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            query = query.OrderBy(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            query = query.OrderBy(q => q.TaxTypeId);
                            break;
                        case ProductOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            query = query.OrderBy(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            query = query.OrderBy(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case ProductOrder.IsPurchasable:
                            query = query.OrderBy(q => q.IsPurchasable);
                            break;
                        case ProductOrder.IsSellable:
                            query = query.OrderBy(q => q.IsSellable);
                            break;
                        case ProductOrder.IsNew:
                            query = query.OrderBy(q => q.IsNew);
                            break;
                        case ProductOrder.UsedVariation:
                            query = query.OrderBy(q => q.UsedVariationId);
                            break;
                        case ProductOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                        case ProductOrder.IsCopyRightedProduct:
                            query = query.OrderBy(q => q.IsCopyRightedProduct);
                            break;
                        case ProductOrder.IsBrandProduct:
                            query = query.OrderBy(q => q.IsBrandProduct);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProductOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProductOrder.SupplierCode:
                            query = query.OrderByDescending(q => q.SupplierCode);
                            break;
                        case ProductOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            query = query.OrderByDescending(q => q.ScanCode);
                            break;
                        case ProductOrder.ERPCode:
                            query = query.OrderByDescending(q => q.ERPCode);
                            break;
                        case ProductOrder.Category:
                            query = query.OrderByDescending(q => q.CategoryId);
                            break;
                        case ProductOrder.ProductType:
                            query = query.OrderByDescending(q => q.ProductTypeId);
                            break;
                        case ProductOrder.Brand:
                            query = query.OrderByDescending(q => q.BrandId);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case ProductOrder.CodeGeneratorRule:
                            query = query.OrderByDescending(q => q.CodeGeneratorRuleId);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            query = query.OrderByDescending(q => q.UnitOfMeasureGroupingId);
                            break;
                        case ProductOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            query = query.OrderByDescending(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            query = query.OrderByDescending(q => q.TaxTypeId);
                            break;
                        case ProductOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            query = query.OrderByDescending(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            query = query.OrderByDescending(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case ProductOrder.IsPurchasable:
                            query = query.OrderByDescending(q => q.IsPurchasable);
                            break;
                        case ProductOrder.IsSellable:
                            query = query.OrderByDescending(q => q.IsSellable);
                            break;
                        case ProductOrder.IsNew:
                            query = query.OrderByDescending(q => q.IsNew);
                            break;
                        case ProductOrder.UsedVariation:
                            query = query.OrderByDescending(q => q.UsedVariationId);
                            break;
                        case ProductOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                        case ProductOrder.IsCopyRightedProduct:
                            query = query.OrderByDescending(q => q.IsCopyRightedProduct);
                            break;
                        case ProductOrder.IsBrandProduct:
                            query = query.OrderByDescending(q => q.IsBrandProduct);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Product>> DynamicSelect(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            List<Product> Products = await query.Select(q => new Product()
            {
                Id = filter.Selects.Contains(ProductSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProductSelect.Code) ? q.Code : default(string),
                SupplierCode = filter.Selects.Contains(ProductSelect.SupplierCode) ? q.SupplierCode : default(string),
                Name = filter.Selects.Contains(ProductSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(ProductSelect.Description) ? q.Description : default(string),
                ScanCode = filter.Selects.Contains(ProductSelect.ScanCode) ? q.ScanCode : default(string),
                ERPCode = filter.Selects.Contains(ProductSelect.ERPCode) ? q.ERPCode : default(string),
                CategoryId = filter.Selects.Contains(ProductSelect.Category) ? q.CategoryId : default(long),
                ProductTypeId = filter.Selects.Contains(ProductSelect.ProductType) ? q.ProductTypeId : default(long),
                BrandId = filter.Selects.Contains(ProductSelect.Brand) ? q.BrandId : default(long?),
                UnitOfMeasureId = filter.Selects.Contains(ProductSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                CodeGeneratorRuleId = filter.Selects.Contains(ProductSelect.CodeGeneratorRule) ? q.CodeGeneratorRuleId : default(long?),
                UnitOfMeasureGroupingId = filter.Selects.Contains(ProductSelect.UnitOfMeasureGrouping) ? q.UnitOfMeasureGroupingId : default(long?),
                SalePrice = filter.Selects.Contains(ProductSelect.SalePrice) ? q.SalePrice : default(decimal?),
                RetailPrice = filter.Selects.Contains(ProductSelect.RetailPrice) ? q.RetailPrice : default(decimal?),
                TaxTypeId = filter.Selects.Contains(ProductSelect.TaxType) ? q.TaxTypeId : default(long),
                StatusId = filter.Selects.Contains(ProductSelect.Status) ? q.StatusId : default(long),
                OtherName = filter.Selects.Contains(ProductSelect.OtherName) ? q.OtherName : default(string),
                TechnicalName = filter.Selects.Contains(ProductSelect.TechnicalName) ? q.TechnicalName : default(string),
                Note = filter.Selects.Contains(ProductSelect.Note) ? q.Note : default(string),
                IsPurchasable = filter.Selects.Contains(ProductSelect.IsPurchasable) ? q.IsPurchasable : default(bool),
                IsSellable = filter.Selects.Contains(ProductSelect.IsSellable) ? q.IsSellable : default(bool),
                IsNew = filter.Selects.Contains(ProductSelect.IsNew) ? q.IsNew : default(bool),
                UsedVariationId = filter.Selects.Contains(ProductSelect.UsedVariation) ? q.UsedVariationId : default(long),
                Used = filter.Selects.Contains(ProductSelect.Used) ? q.Used : default(bool),
                IsCopyRightedProduct = filter.Selects.Contains(ProductSelect.IsCopyRightedProduct) ? q.IsCopyRightedProduct : default(bool),
                IsBrandProduct = filter.Selects.Contains(ProductSelect.IsBrandProduct) ? q.IsBrandProduct : default(bool),
                Brand = filter.Selects.Contains(ProductSelect.Brand) && q.Brand != null ? new Brand
                {
                    Id = q.Brand.Id,
                    Code = q.Brand.Code,
                    Name = q.Brand.Name,
                    StatusId = q.Brand.StatusId,
                    Description = q.Brand.Description,
                    Used = q.Brand.Used,
                } : null,
                Category = filter.Selects.Contains(ProductSelect.Category) && q.Category != null ? new Category
                {
                    Id = q.Category.Id,
                    Code = q.Category.Code,
                    Name = q.Category.Name,
                    Prefix = q.Category.Prefix,
                    Description = q.Category.Description,
                    ParentId = q.Category.ParentId,
                    Path = q.Category.Path,
                    Level = q.Category.Level,
                    HasChildren = q.Category.HasChildren,
                    StatusId = q.Category.StatusId,
                    ImageId = q.Category.ImageId,
                    Used = q.Category.Used,
                    OrderNumber = q.Category.OrderNumber,
                } : null,
                ProductType = filter.Selects.Contains(ProductSelect.ProductType) && q.ProductType != null ? new ProductType
                {
                    Id = q.ProductType.Id,
                    Code = q.ProductType.Code,
                    Name = q.ProductType.Name,
                    Description = q.ProductType.Description,
                    StatusId = q.ProductType.StatusId,
                    Used = q.ProductType.Used,
                } : null,
                Status = filter.Selects.Contains(ProductSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                    Color = q.Status.Color,
                } : null,
                TaxType = filter.Selects.Contains(ProductSelect.TaxType) && q.TaxType != null ? new TaxType
                {
                    Id = q.TaxType.Id,
                    Code = q.TaxType.Code,
                    Name = q.TaxType.Name,
                    Percentage = q.TaxType.Percentage,
                    StatusId = q.TaxType.StatusId,
                    Used = q.TaxType.Used,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(ProductSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    IsDecimal = q.UnitOfMeasure.IsDecimal,
                    Used = q.UnitOfMeasure.Used,
                    ErpCode = q.UnitOfMeasure.ErpCode,
                } : null,
                UnitOfMeasureGrouping = filter.Selects.Contains(ProductSelect.UnitOfMeasureGrouping) && q.UnitOfMeasureGrouping != null ? new UnitOfMeasureGrouping
                {
                    Id = q.UnitOfMeasureGrouping.Id,
                    Code = q.UnitOfMeasureGrouping.Code,
                    Name = q.UnitOfMeasureGrouping.Name,
                    Description = q.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = q.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = q.UnitOfMeasureGrouping.StatusId,
                    Used = q.UnitOfMeasureGrouping.Used,
                } : null,
                RowId = q.RowId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListAsync();
            return Products;
        }

        public async Task<int> CountAll(ProductFilter filter)
        {
            IQueryable<ProductDAO> ProductDAOs = DataContext.Product.AsNoTracking();
            ProductDAOs = await DynamicFilter(ProductDAOs, filter);
            return await ProductDAOs.CountAsync();
        }

        public async Task<int> Count(ProductFilter filter)
        {
            IQueryable<ProductDAO> ProductDAOs = DataContext.Product.AsNoTracking();
            ProductDAOs = await DynamicFilter(ProductDAOs, filter);
            ProductDAOs = await OrFilter(ProductDAOs, filter);
            return await ProductDAOs.CountAsync();
        }

        public async Task<List<Product>> List(ProductFilter filter)
        {
            if (filter == null) return new List<Product>();
            IQueryable<ProductDAO> ProductDAOs = DataContext.Product.AsNoTracking();
            ProductDAOs = await DynamicFilter(ProductDAOs, filter);
            ProductDAOs = await OrFilter(ProductDAOs, filter);
            ProductDAOs = DynamicOrder(ProductDAOs, filter);
            List<Product> Products = await DynamicSelect(ProductDAOs, filter);
            return Products;
        }

        public async Task<List<Product>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<ProductDAO> query = DataContext.Product.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Product> Products = await query.AsNoTracking()
            .Select(x => new Product()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                SupplierCode = x.SupplierCode,
                Name = x.Name,
                Description = x.Description,
                ScanCode = x.ScanCode,
                ERPCode = x.ERPCode,
                CategoryId = x.CategoryId,
                ProductTypeId = x.ProductTypeId,
                BrandId = x.BrandId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                CodeGeneratorRuleId = x.CodeGeneratorRuleId,
                UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                SalePrice = x.SalePrice,
                RetailPrice = x.RetailPrice,
                TaxTypeId = x.TaxTypeId,
                StatusId = x.StatusId,
                OtherName = x.OtherName,
                TechnicalName = x.TechnicalName,
                Note = x.Note,
                IsPurchasable = x.IsPurchasable,
                IsSellable = x.IsSellable,
                IsNew = x.IsNew,
                UsedVariationId = x.UsedVariationId,
                Used = x.Used,
                IsCopyRightedProduct = x.IsCopyRightedProduct,
                IsBrandProduct = x.IsBrandProduct,
                Brand = x.Brand == null ? null : new Brand
                {
                    Id = x.Brand.Id,
                    Code = x.Brand.Code,
                    Name = x.Brand.Name,
                    StatusId = x.Brand.StatusId,
                    Description = x.Brand.Description,
                    Used = x.Brand.Used,
                },
                Category = x.Category == null ? null : new Category
                {
                    Id = x.Category.Id,
                    Code = x.Category.Code,
                    Name = x.Category.Name,
                    Prefix = x.Category.Prefix,
                    Description = x.Category.Description,
                    ParentId = x.Category.ParentId,
                    Path = x.Category.Path,
                    Level = x.Category.Level,
                    HasChildren = x.Category.HasChildren,
                    StatusId = x.Category.StatusId,
                    ImageId = x.Category.ImageId,
                    Used = x.Category.Used,
                    OrderNumber = x.Category.OrderNumber,
                },
                ProductType = x.ProductType == null ? null : new ProductType
                {
                    Id = x.ProductType.Id,
                    Code = x.ProductType.Code,
                    Name = x.ProductType.Name,
                    Description = x.ProductType.Description,
                    StatusId = x.ProductType.StatusId,
                    Used = x.ProductType.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
                TaxType = x.TaxType == null ? null : new TaxType
                {
                    Id = x.TaxType.Id,
                    Code = x.TaxType.Code,
                    Name = x.TaxType.Name,
                    Percentage = x.TaxType.Percentage,
                    StatusId = x.TaxType.StatusId,
                    Used = x.TaxType.Used,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    IsDecimal = x.UnitOfMeasure.IsDecimal,
                    Used = x.UnitOfMeasure.Used,
                    ErpCode = x.UnitOfMeasure.ErpCode,
                },
                UnitOfMeasureGrouping = x.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                {
                    Id = x.UnitOfMeasureGrouping.Id,
                    Code = x.UnitOfMeasureGrouping.Code,
                    Name = x.UnitOfMeasureGrouping.Name,
                    Description = x.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = x.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = x.UnitOfMeasureGrouping.StatusId,
                    Used = x.UnitOfMeasureGrouping.Used,
                },
            }).ToListAsync();
            

            return Products;
        }

        public async Task<Product> Get(long Id)
        {
            Product Product = await DataContext.Product.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Product()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                SupplierCode = x.SupplierCode,
                Name = x.Name,
                Description = x.Description,
                ScanCode = x.ScanCode,
                ERPCode = x.ERPCode,
                CategoryId = x.CategoryId,
                ProductTypeId = x.ProductTypeId,
                BrandId = x.BrandId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                CodeGeneratorRuleId = x.CodeGeneratorRuleId,
                UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                SalePrice = x.SalePrice,
                RetailPrice = x.RetailPrice,
                TaxTypeId = x.TaxTypeId,
                StatusId = x.StatusId,
                OtherName = x.OtherName,
                TechnicalName = x.TechnicalName,
                Note = x.Note,
                IsPurchasable = x.IsPurchasable,
                IsSellable = x.IsSellable,
                IsNew = x.IsNew,
                UsedVariationId = x.UsedVariationId,
                Used = x.Used,
                IsCopyRightedProduct = x.IsCopyRightedProduct,
                IsBrandProduct = x.IsBrandProduct,
                Brand = x.Brand == null ? null : new Brand
                {
                    Id = x.Brand.Id,
                    Code = x.Brand.Code,
                    Name = x.Brand.Name,
                    StatusId = x.Brand.StatusId,
                    Description = x.Brand.Description,
                    Used = x.Brand.Used,
                },
                Category = x.Category == null ? null : new Category
                {
                    Id = x.Category.Id,
                    Code = x.Category.Code,
                    Name = x.Category.Name,
                    Prefix = x.Category.Prefix,
                    Description = x.Category.Description,
                    ParentId = x.Category.ParentId,
                    Path = x.Category.Path,
                    Level = x.Category.Level,
                    HasChildren = x.Category.HasChildren,
                    StatusId = x.Category.StatusId,
                    ImageId = x.Category.ImageId,
                    Used = x.Category.Used,
                    OrderNumber = x.Category.OrderNumber,
                },
                ProductType = x.ProductType == null ? null : new ProductType
                {
                    Id = x.ProductType.Id,
                    Code = x.ProductType.Code,
                    Name = x.ProductType.Name,
                    Description = x.ProductType.Description,
                    StatusId = x.ProductType.StatusId,
                    Used = x.ProductType.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
                TaxType = x.TaxType == null ? null : new TaxType
                {
                    Id = x.TaxType.Id,
                    Code = x.TaxType.Code,
                    Name = x.TaxType.Name,
                    Percentage = x.TaxType.Percentage,
                    StatusId = x.TaxType.StatusId,
                    Used = x.TaxType.Used,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    IsDecimal = x.UnitOfMeasure.IsDecimal,
                    Used = x.UnitOfMeasure.Used,
                    ErpCode = x.UnitOfMeasure.ErpCode,
                },
                UnitOfMeasureGrouping = x.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                {
                    Id = x.UnitOfMeasureGrouping.Id,
                    Code = x.UnitOfMeasureGrouping.Code,
                    Name = x.UnitOfMeasureGrouping.Name,
                    Description = x.UnitOfMeasureGrouping.Description,
                    UnitOfMeasureId = x.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = x.UnitOfMeasureGrouping.StatusId,
                    Used = x.UnitOfMeasureGrouping.Used,
                },
            }).FirstOrDefaultAsync();

            if (Product == null)
                return null;

            return Product;
        }

        public async Task<List<long>> BulkMerge(List<Product> Products)
        {
            IdFilter IdFilter = new IdFilter { In = Products.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<ProductDAO> ProductDAOs = new List<ProductDAO>();
            foreach (Product Product in Products)
            {
                ProductDAO ProductDAO = new ProductDAO();
                ProductDAOs.Add(ProductDAO);
                ProductDAO.Id = Product.Id;
                ProductDAO.Code = Product.Code;
                ProductDAO.SupplierCode = Product.SupplierCode;
                ProductDAO.Name = Product.Name;
                ProductDAO.Description = Product.Description;
                ProductDAO.ScanCode = Product.ScanCode;
                ProductDAO.ERPCode = Product.ERPCode;
                ProductDAO.CategoryId = Product.CategoryId;
                ProductDAO.ProductTypeId = Product.ProductTypeId;
                ProductDAO.BrandId = Product.BrandId;
                ProductDAO.UnitOfMeasureId = Product.UnitOfMeasureId;
                ProductDAO.CodeGeneratorRuleId = Product.CodeGeneratorRuleId;
                ProductDAO.UnitOfMeasureGroupingId = Product.UnitOfMeasureGroupingId;
                ProductDAO.SalePrice = Product.SalePrice;
                ProductDAO.RetailPrice = Product.RetailPrice;
                ProductDAO.TaxTypeId = Product.TaxTypeId;
                ProductDAO.StatusId = Product.StatusId;
                ProductDAO.OtherName = Product.OtherName;
                ProductDAO.TechnicalName = Product.TechnicalName;
                ProductDAO.Note = Product.Note;
                ProductDAO.IsPurchasable = Product.IsPurchasable;
                ProductDAO.IsSellable = Product.IsSellable;
                ProductDAO.IsNew = Product.IsNew;
                ProductDAO.UsedVariationId = Product.UsedVariationId;
                ProductDAO.Used = Product.Used;
                ProductDAO.IsCopyRightedProduct = Product.IsCopyRightedProduct;
                ProductDAO.IsBrandProduct = Product.IsBrandProduct;
                ProductDAO.CreatedAt = Product.CreatedAt;
                ProductDAO.UpdatedAt = Product.UpdatedAt;
                ProductDAO.DeletedAt = Product.DeletedAt;
                ProductDAO.RowId = Product.RowId;
            }
            await DataContext.BulkMergeAsync(ProductDAOs);
            var Ids = ProductDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Product
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new ProductDAO { Used = true });
            return true;
        }
    }
}
