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
    public interface ICategoryRepository
    {
        Task<int> CountAll(CategoryFilter CategoryFilter);
        Task<int> Count(CategoryFilter CategoryFilter);
        Task<List<Category>> List(CategoryFilter CategoryFilter);
        Task<List<Category>> List(List<long> Ids);
        Task<Category> Get(long Id);
        Task<List<long>> BulkMerge(List<Category> Categories);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext DataContext;
        public CategoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<CategoryDAO>> DynamicFilter(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Prefix, filter.Prefix);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.HasChildren, filter.HasChildren);
            query = query.Where(q => q.OrderNumber, filter.OrderNumber);
            query = query.Where(q => q.ImageId, filter.ImageId);
            if (filter.ParentId != null && filter.ParentId.HasValue)
            {
                if (filter.ParentId.Equal != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.ParentId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Parent.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.ParentId.NotEqual != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.ParentId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Parent.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.ParentId.In != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.ParentId.In.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.ParentId, IdFilter);
                }
                if (filter.ParentId.NotIn != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.ParentId.NotIn.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.ParentId, IdFilter);
                }
            }
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(CategorySearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(CategorySearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<CategoryDAO>> OrFilter(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<CategoryDAO> initQuery = query.Where(q => false);
            foreach (CategoryFilter CategoryFilter in filter.OrFilter)
            {
                IQueryable<CategoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, CategoryFilter.Id);
                queryable = queryable.Where(q => q.Code, CategoryFilter.Code);
                queryable = queryable.Where(q => q.Name, CategoryFilter.Name);
                queryable = queryable.Where(q => q.Prefix, CategoryFilter.Prefix);
                queryable = queryable.Where(q => q.Description, CategoryFilter.Description);
                queryable = queryable.Where(q => q.Path, CategoryFilter.Path);
                queryable = queryable.Where(q => q.Level, CategoryFilter.Level);
                queryable = queryable.Where(q => q.HasChildren, CategoryFilter.HasChildren);
                queryable = queryable.Where(q => q.OrderNumber, CategoryFilter.OrderNumber);
                queryable = queryable.Where(q => q.ImageId, CategoryFilter.ImageId);
                if (CategoryFilter.ParentId != null && CategoryFilter.ParentId.HasValue)
                {
                    if (CategoryFilter.ParentId.Equal != null)
                    {
                        CategoryDAO CategoryDAO = DataContext.Category
                            .Where(o => o.Id == CategoryFilter.ParentId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Parent.Path.StartsWith(CategoryDAO.Path));
                    }
                    if (CategoryFilter.ParentId.NotEqual != null)
                    {
                        CategoryDAO CategoryDAO = DataContext.Category
                            .Where(o => o.Id == CategoryFilter.ParentId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Parent.Path.StartsWith(CategoryDAO.Path));
                    }
                    if (CategoryFilter.ParentId.In != null)
                    {
                        List<CategoryDAO> CategoryDAOs = DataContext.Category
                            .Where(o => o.DeletedAt == null).ToList();
                        List<CategoryDAO> Parents = CategoryDAOs.Where(o => CategoryFilter.ParentId.In.Contains(o.Id)).ToList();
                        List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.ParentId, IdFilter);
                    }
                    if (CategoryFilter.ParentId.NotIn != null)
                    {
                        List<CategoryDAO> CategoryDAOs = DataContext.Category
                            .Where(o => o.DeletedAt == null).ToList();
                        List<CategoryDAO> Parents = CategoryDAOs.Where(o => CategoryFilter.ParentId.NotIn.Contains(o.Id)).ToList();
                        List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.ParentId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.StatusId, CategoryFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<CategoryDAO> DynamicOrder(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case CategoryOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case CategoryOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case CategoryOrder.Prefix:
                            query = query.OrderBy(q => q.Prefix);
                            break;
                        case CategoryOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case CategoryOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case CategoryOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case CategoryOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case CategoryOrder.HasChildren:
                            query = query.OrderBy(q => q.HasChildren);
                            break;
                        case CategoryOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case CategoryOrder.Image:
                            query = query.OrderBy(q => q.ImageId);
                            break;
                        case CategoryOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                        case CategoryOrder.OrderNumber:
                            query = query.OrderBy(q => q.OrderNumber);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case CategoryOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case CategoryOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case CategoryOrder.Prefix:
                            query = query.OrderByDescending(q => q.Prefix);
                            break;
                        case CategoryOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case CategoryOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case CategoryOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case CategoryOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case CategoryOrder.HasChildren:
                            query = query.OrderByDescending(q => q.HasChildren);
                            break;
                        case CategoryOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case CategoryOrder.Image:
                            query = query.OrderByDescending(q => q.ImageId);
                            break;
                        case CategoryOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                        case CategoryOrder.OrderNumber:
                            query = query.OrderByDescending(q => q.OrderNumber);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Category>> DynamicSelect(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            List<Category> Categories = await query.Select(q => new Category()
            {
                Id = filter.Selects.Contains(CategorySelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(CategorySelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(CategorySelect.Name) ? q.Name : default(string),
                Prefix = filter.Selects.Contains(CategorySelect.Prefix) ? q.Prefix : default(string),
                Description = filter.Selects.Contains(CategorySelect.Description) ? q.Description : default(string),
                ParentId = filter.Selects.Contains(CategorySelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(CategorySelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(CategorySelect.Level) ? q.Level : default(long),
                HasChildren = filter.Selects.Contains(CategorySelect.HasChildren) ? q.HasChildren : default(bool),
                StatusId = filter.Selects.Contains(CategorySelect.Status) ? q.StatusId : default(long),
                ImageId = filter.Selects.Contains(CategorySelect.Image) ? q.ImageId : default(long?),
                Used = filter.Selects.Contains(CategorySelect.Used) ? q.Used : default(bool),
                OrderNumber = filter.Selects.Contains(CategorySelect.OrderNumber) ? q.OrderNumber : default(long),
                Image = filter.Selects.Contains(CategorySelect.Image) && q.Image != null ? new Image
                {
                    Id = q.Image.Id,
                    Name = q.Image.Name,
                    Url = q.Image.Url,
                    ThumbnailUrl = q.Image.ThumbnailUrl,
                } : null,
                Parent = filter.Selects.Contains(CategorySelect.Parent) && q.Parent != null ? new Category
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    Prefix = q.Parent.Prefix,
                    Description = q.Parent.Description,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Level = q.Parent.Level,
                    HasChildren = q.Parent.HasChildren,
                    StatusId = q.Parent.StatusId,
                    ImageId = q.Parent.ImageId,
                    Used = q.Parent.Used,
                    OrderNumber = q.Parent.OrderNumber,
                } : null,
                Status = filter.Selects.Contains(CategorySelect.Status) && q.Status != null ? new Status
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
            return Categories;
        }

        public async Task<int> CountAll(CategoryFilter filter)
        {
            IQueryable<CategoryDAO> CategoryDAOs = DataContext.Category.AsNoTracking();
            CategoryDAOs = await DynamicFilter(CategoryDAOs, filter);
            return await CategoryDAOs.CountAsync();
        }

        public async Task<int> Count(CategoryFilter filter)
        {
            IQueryable<CategoryDAO> CategoryDAOs = DataContext.Category.AsNoTracking();
            CategoryDAOs = await DynamicFilter(CategoryDAOs, filter);
            CategoryDAOs = await OrFilter(CategoryDAOs, filter);
            return await CategoryDAOs.CountAsync();
        }

        public async Task<List<Category>> List(CategoryFilter filter)
        {
            if (filter == null) return new List<Category>();
            IQueryable<CategoryDAO> CategoryDAOs = DataContext.Category.AsNoTracking();
            CategoryDAOs = await DynamicFilter(CategoryDAOs, filter);
            CategoryDAOs = await OrFilter(CategoryDAOs, filter);
            CategoryDAOs = DynamicOrder(CategoryDAOs, filter);
            List<Category> Categories = await DynamicSelect(CategoryDAOs, filter);
            return Categories;
        }

        public async Task<List<Category>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<CategoryDAO> query = DataContext.Category.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Category> Categories = await query.AsNoTracking()
            .Select(x => new Category()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Prefix = x.Prefix,
                Description = x.Description,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                HasChildren = x.HasChildren,
                StatusId = x.StatusId,
                ImageId = x.ImageId,
                Used = x.Used,
                OrderNumber = x.OrderNumber,
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                },
                Parent = x.Parent == null ? null : new Category
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    Prefix = x.Parent.Prefix,
                    Description = x.Parent.Description,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    HasChildren = x.Parent.HasChildren,
                    StatusId = x.Parent.StatusId,
                    ImageId = x.Parent.ImageId,
                    Used = x.Parent.Used,
                    OrderNumber = x.Parent.OrderNumber,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return Categories;
        }

        public async Task<Category> Get(long Id)
        {
            Category Category = await DataContext.Category.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Category()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Prefix = x.Prefix,
                Description = x.Description,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                HasChildren = x.HasChildren,
                StatusId = x.StatusId,
                ImageId = x.ImageId,
                Used = x.Used,
                OrderNumber = x.OrderNumber,
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                },
                Parent = x.Parent == null ? null : new Category
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    Prefix = x.Parent.Prefix,
                    Description = x.Parent.Description,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    HasChildren = x.Parent.HasChildren,
                    StatusId = x.Parent.StatusId,
                    ImageId = x.Parent.ImageId,
                    Used = x.Parent.Used,
                    OrderNumber = x.Parent.OrderNumber,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (Category == null)
                return null;

            return Category;
        }

        public async Task<List<long>> BulkMerge(List<Category> Categories)
        {
            IdFilter IdFilter = new IdFilter { In = Categories.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<CategoryDAO> CategoryDAOs = new List<CategoryDAO>();
            foreach (Category Category in Categories)
            {
                CategoryDAO CategoryDAO = new CategoryDAO();
                CategoryDAOs.Add(CategoryDAO);
                CategoryDAO.Id = Category.Id;
                CategoryDAO.Code = Category.Code;
                CategoryDAO.Name = Category.Name;
                CategoryDAO.Prefix = Category.Prefix;
                CategoryDAO.Description = Category.Description;
                CategoryDAO.ParentId = Category.ParentId;
                CategoryDAO.Path = Category.Path;
                CategoryDAO.Level = Category.Level;
                CategoryDAO.HasChildren = Category.HasChildren;
                CategoryDAO.StatusId = Category.StatusId;
                CategoryDAO.ImageId = Category.ImageId;
                CategoryDAO.Used = Category.Used;
                CategoryDAO.OrderNumber = Category.OrderNumber;
                CategoryDAO.CreatedAt = Category.CreatedAt;
                CategoryDAO.UpdatedAt = Category.UpdatedAt;
                CategoryDAO.DeletedAt = Category.DeletedAt;
                CategoryDAO.RowId = Category.RowId;
            }
            await DataContext.BulkMergeAsync(CategoryDAOs);
            var Ids = CategoryDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        private async Task BuildPath()
        {
            List<CategoryDAO> CategoryDAOs = await DataContext.Category
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
            Queue<CategoryDAO> queue = new Queue<CategoryDAO>();
            CategoryDAOs.ForEach(x =>
            {
                x.HasChildren = false;
                if (!x.ParentId.HasValue)
                {
                    x.Path = x.Id + ".";
                    x.Level = 1;
                    queue.Enqueue(x);
                }
            });
            while(queue.Count > 0)
            {
                CategoryDAO Parent = queue.Dequeue();
                foreach (CategoryDAO CategoryDAO in CategoryDAOs)
                {
                    if (CategoryDAO.ParentId == Parent.Id)
                    {
                        Parent.HasChildren = true;
                        CategoryDAO.Path = Parent.Path + CategoryDAO.Id + ".";
                        CategoryDAO.Level = Parent.Level + 1;
                        queue.Enqueue(CategoryDAO);
                    }
                }
            }
            await DataContext.SaveChangesAsync();
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Category
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new CategoryDAO { Used = true });
            return true;
        }
    }
}
