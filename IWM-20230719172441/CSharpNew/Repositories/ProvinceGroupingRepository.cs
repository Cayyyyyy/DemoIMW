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
    public interface IProvinceGroupingRepository
    {
        Task<int> CountAll(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<int> Count(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<List<ProvinceGrouping>> List(ProvinceGroupingFilter ProvinceGroupingFilter);
        Task<List<ProvinceGrouping>> List(List<long> Ids);
        Task<ProvinceGrouping> Get(long Id);
        Task<bool> Create(ProvinceGrouping ProvinceGrouping);
        Task<bool> Update(ProvinceGrouping ProvinceGrouping);
        Task<bool> Delete(ProvinceGrouping ProvinceGrouping);
        Task<List<long>> BulkMerge(List<ProvinceGrouping> ProvinceGroupings);
        Task<bool> BulkDelete(List<ProvinceGrouping> ProvinceGroupings);
    }
    public class ProvinceGroupingRepository : IProvinceGroupingRepository
    {
        private readonly DataContext DataContext;
        public ProvinceGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<ProvinceGroupingDAO>> DynamicFilter(IQueryable<ProvinceGroupingDAO> query, ProvinceGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.HasChildren, filter.HasChildren);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.ParentId, filter.ParentId, DataContext.ProvinceGrouping.AsNoTracking());
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(ProvinceGroupingSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(ProvinceGroupingSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<ProvinceGroupingDAO>> OrFilter(IQueryable<ProvinceGroupingDAO> query, ProvinceGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (ProvinceGroupingFilter ProvinceGroupingFilter in filter.OrFilter)
            {
                IQueryable<ProvinceGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProvinceGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, ProvinceGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, ProvinceGroupingFilter.Name);
                queryable = queryable.Where(q => q.HasChildren, ProvinceGroupingFilter.HasChildren);
                queryable = queryable.Where(q => q.Level, ProvinceGroupingFilter.Level);
                queryable = queryable.Where(q => q.Path, ProvinceGroupingFilter.Path);
                queryable = queryable.Where(q => q.ParentId, ProvinceGroupingFilter.ParentId, DataContext.ProvinceGrouping.AsNoTracking());
                queryable = queryable.Where(q => q.StatusId, ProvinceGroupingFilter.StatusId);
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

        private IQueryable<ProvinceGroupingDAO> DynamicOrder(IQueryable<ProvinceGroupingDAO> query, ProvinceGroupingFilter filter)
        {
            Dictionary<ProvinceGroupingOrder, LambdaExpression> CustomOrder = new Dictionary<ProvinceGroupingOrder, LambdaExpression>()
            {
                { ProvinceGroupingOrder.Parent, (ProvinceGroupingDAO x) => x.Parent.Name  },
                { ProvinceGroupingOrder.Status, (ProvinceGroupingDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<ProvinceGrouping>> DynamicSelect(IQueryable<ProvinceGroupingDAO> query, ProvinceGroupingFilter filter)
        {
            List<ProvinceGrouping> ProvinceGroupings = await query.Select(q => new ProvinceGrouping()
            {
                Id = filter.Selects.Contains(ProvinceGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProvinceGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProvinceGroupingSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(ProvinceGroupingSelect.Status) ? q.StatusId : default(long),
                ParentId = filter.Selects.Contains(ProvinceGroupingSelect.Parent) ? q.ParentId : default(long?),
                HasChildren = filter.Selects.Contains(ProvinceGroupingSelect.HasChildren) ? q.HasChildren : default(bool),
                Level = filter.Selects.Contains(ProvinceGroupingSelect.Level) ? q.Level : default(long),
                Path = filter.Selects.Contains(ProvinceGroupingSelect.Path) ? q.Path : default(string),
                Parent = filter.Selects.Contains(ProvinceGroupingSelect.Parent) && q.Parent != null ? new ProvinceGrouping
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    StatusId = q.Parent.StatusId,
                    ParentId = q.Parent.ParentId,
                    HasChildren = q.Parent.HasChildren,
                    Level = q.Parent.Level,
                    Path = q.Parent.Path,
                } : null,
                Status = filter.Selects.Contains(ProvinceGroupingSelect.Status) && q.Status != null ? new Status
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
            return ProvinceGroupings;
        }

        public async Task<int> CountAll(ProvinceGroupingFilter filter)
        {
            IQueryable<ProvinceGroupingDAO> ProvinceGroupingDAOs = DataContext.ProvinceGrouping.AsNoTracking();
            ProvinceGroupingDAOs = await DynamicFilter(ProvinceGroupingDAOs, filter);
            return await ProvinceGroupingDAOs.CountAsync();
        }

        public async Task<int> Count(ProvinceGroupingFilter filter)
        {
            IQueryable<ProvinceGroupingDAO> ProvinceGroupingDAOs = DataContext.ProvinceGrouping.AsNoTracking();
            ProvinceGroupingDAOs = await DynamicFilter(ProvinceGroupingDAOs, filter);
            ProvinceGroupingDAOs = await OrFilter(ProvinceGroupingDAOs, filter);
            return await ProvinceGroupingDAOs.CountAsync();
        }

        public async Task<List<ProvinceGrouping>> List(ProvinceGroupingFilter filter)
        {
            if (filter == null) return new List<ProvinceGrouping>();
            IQueryable<ProvinceGroupingDAO> ProvinceGroupingDAOs = DataContext.ProvinceGrouping.AsNoTracking();
            ProvinceGroupingDAOs = await DynamicFilter(ProvinceGroupingDAOs, filter);
            ProvinceGroupingDAOs = await OrFilter(ProvinceGroupingDAOs, filter);
            ProvinceGroupingDAOs = DynamicOrder(ProvinceGroupingDAOs, filter);
            List<ProvinceGrouping> ProvinceGroupings = await DynamicSelect(ProvinceGroupingDAOs, filter);
            return ProvinceGroupings;
        }

        public async Task<List<ProvinceGrouping>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<ProvinceGroupingDAO> query = DataContext.ProvinceGrouping.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<ProvinceGrouping> ProvinceGroupings = await query.AsNoTracking()
            .Select(x => new ProvinceGrouping()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                ParentId = x.ParentId,
                HasChildren = x.HasChildren,
                Level = x.Level,
                Path = x.Path,
                Parent = x.Parent == null ? null : new ProvinceGrouping
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    StatusId = x.Parent.StatusId,
                    ParentId = x.Parent.ParentId,
                    HasChildren = x.Parent.HasChildren,
                    Level = x.Parent.Level,
                    Path = x.Parent.Path,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();


            return ProvinceGroupings;
        }

        public async Task<ProvinceGrouping> Get(long Id)
        {
            ProvinceGrouping ProvinceGrouping = await DataContext.ProvinceGrouping.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ProvinceGrouping()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                ParentId = x.ParentId,
                HasChildren = x.HasChildren,
                Level = x.Level,
                Path = x.Path,
                Parent = x.Parent == null ? null : new ProvinceGrouping
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    StatusId = x.Parent.StatusId,
                    ParentId = x.Parent.ParentId,
                    HasChildren = x.Parent.HasChildren,
                    Level = x.Parent.Level,
                    Path = x.Parent.Path,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (ProvinceGrouping == null)
                return null;

            return ProvinceGrouping;
        }
        public async Task<bool> Create(ProvinceGrouping ProvinceGrouping)
        {
            ProvinceGroupingDAO ProvinceGroupingDAO = new ProvinceGroupingDAO();
            ProvinceGroupingDAO.Id = ProvinceGrouping.Id;
            ProvinceGroupingDAO.Code = ProvinceGrouping.Code;
            ProvinceGroupingDAO.Name = ProvinceGrouping.Name;
            ProvinceGroupingDAO.StatusId = ProvinceGrouping.StatusId;
            ProvinceGroupingDAO.ParentId = ProvinceGrouping.ParentId;
            ProvinceGroupingDAO.HasChildren = ProvinceGrouping.HasChildren;
            ProvinceGroupingDAO.Level = ProvinceGrouping.Level;
            ProvinceGroupingDAO.Path = ProvinceGrouping.Path;
            ProvinceGroupingDAO.RowId = Guid.NewGuid();
            ProvinceGroupingDAO.Path = "";
            ProvinceGroupingDAO.Level = 1;
            ProvinceGroupingDAO.CreatedAt = StaticParams.DateTimeNow;
            ProvinceGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ProvinceGrouping.Add(ProvinceGroupingDAO);
            await DataContext.SaveChangesAsync();
            ProvinceGrouping.Id = ProvinceGroupingDAO.Id;
            await SaveReference(ProvinceGrouping);
            await BuildPath();
            return true;
        }

        public async Task<bool> Update(ProvinceGrouping ProvinceGrouping)
        {
            ProvinceGroupingDAO ProvinceGroupingDAO = DataContext.ProvinceGrouping
                .Where(x => x.Id == ProvinceGrouping.Id)
                .FirstOrDefault();
            if (ProvinceGroupingDAO == null)
                return false;
            ProvinceGroupingDAO.Id = ProvinceGrouping.Id;
            ProvinceGroupingDAO.Code = ProvinceGrouping.Code;
            ProvinceGroupingDAO.Name = ProvinceGrouping.Name;
            ProvinceGroupingDAO.StatusId = ProvinceGrouping.StatusId;
            ProvinceGroupingDAO.ParentId = ProvinceGrouping.ParentId;
            ProvinceGroupingDAO.HasChildren = ProvinceGrouping.HasChildren;
            ProvinceGroupingDAO.Level = ProvinceGrouping.Level;
            ProvinceGroupingDAO.Path = ProvinceGrouping.Path;
            ProvinceGroupingDAO.Path = "";
            ProvinceGroupingDAO.Level = 1;
            ProvinceGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ProvinceGrouping);
            await BuildPath();
            return true;
        }

        public async Task<bool> Delete(ProvinceGrouping ProvinceGrouping)
        {
            ProvinceGroupingDAO ProvinceGroupingDAO = await DataContext.ProvinceGrouping.Where(x => x.Id == ProvinceGrouping.Id).FirstOrDefaultAsync();
            await DataContext.ProvinceGrouping
                .Where(x => x.Path.StartsWith(ProvinceGroupingDAO.Id + "."))
                .UpdateFromQueryAsync(x => new ProvinceGroupingDAO 
                {
                    DeletedAt = StaticParams.DateTimeNow, 
                    UpdatedAt = StaticParams.DateTimeNow 
                });
            await DataContext.ProvinceGrouping
                .Where(x => x.Id == ProvinceGrouping.Id)
                .UpdateFromQueryAsync(x => new ProvinceGroupingDAO 
                {
                    DeletedAt = StaticParams.DateTimeNow, 
                    UpdatedAt = StaticParams.DateTimeNow 
                });
            await BuildPath();
            return true;
        }

        public async Task<List<long>> BulkMerge(List<ProvinceGrouping> ProvinceGroupings)
        {
            IdFilter IdFilter = new IdFilter { In = ProvinceGroupings.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<ProvinceGroupingDAO> ProvinceGroupingDAOs = new List<ProvinceGroupingDAO>();
            foreach (ProvinceGrouping ProvinceGrouping in ProvinceGroupings)
            {
                ProvinceGroupingDAO ProvinceGroupingDAO = new ProvinceGroupingDAO();
                ProvinceGroupingDAOs.Add(ProvinceGroupingDAO);
                ProvinceGroupingDAO.Id = ProvinceGrouping.Id;
                ProvinceGroupingDAO.Code = ProvinceGrouping.Code;
                ProvinceGroupingDAO.Name = ProvinceGrouping.Name;
                ProvinceGroupingDAO.StatusId = ProvinceGrouping.StatusId;
                ProvinceGroupingDAO.ParentId = ProvinceGrouping.ParentId;
                ProvinceGroupingDAO.HasChildren = ProvinceGrouping.HasChildren;
                ProvinceGroupingDAO.Level = ProvinceGrouping.Level;
                ProvinceGroupingDAO.Path = ProvinceGrouping.Path;
                ProvinceGroupingDAO.CreatedAt = ProvinceGrouping.CreatedAt;
                ProvinceGroupingDAO.UpdatedAt = ProvinceGrouping.UpdatedAt;
                ProvinceGroupingDAO.RowId = ProvinceGrouping.RowId;
            }
            await DataContext.BulkMergeAsync(ProvinceGroupingDAOs);
            var Ids = ProvinceGroupingDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        
        public async Task<bool> BulkDelete(List<ProvinceGrouping> ProvinceGroupings)
        {
            List<long> Ids = ProvinceGroupings.Select(x => x.Id).ToList();
            await DataContext.ProvinceGrouping
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new ProvinceGroupingDAO 
                {
                    DeletedAt = StaticParams.DateTimeNow, 
                    UpdatedAt = StaticParams.DateTimeNow 
                });
            await BuildPath();
            return true;
        }

        private async Task SaveReference(ProvinceGrouping ProvinceGrouping)
        {
        }

        private async Task BuildPath()
        {
            List<ProvinceGroupingDAO> ProvinceGroupingDAOs = await DataContext.ProvinceGrouping
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
            Queue<ProvinceGroupingDAO> queue = new Queue<ProvinceGroupingDAO>();
            ProvinceGroupingDAOs.ForEach(x =>
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
                ProvinceGroupingDAO Parent = queue.Dequeue();
                foreach (ProvinceGroupingDAO ProvinceGroupingDAO in ProvinceGroupingDAOs)
                {
                    if (ProvinceGroupingDAO.ParentId == Parent.Id)
                    {
                        Parent.HasChildren = true;
                        ProvinceGroupingDAO.Path = Parent.Path + ProvinceGroupingDAO.Id + ".";
                        ProvinceGroupingDAO.Level = Parent.Level + 1;
                        queue.Enqueue(ProvinceGroupingDAO);
                    }
                }
            }
            await DataContext.SaveChangesAsync();
        }
    }
}
