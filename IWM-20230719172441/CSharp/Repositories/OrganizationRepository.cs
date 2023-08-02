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
    public interface IOrganizationRepository
    {
        Task<int> CountAll(OrganizationFilter OrganizationFilter);
        Task<int> Count(OrganizationFilter OrganizationFilter);
        Task<List<Organization>> List(OrganizationFilter OrganizationFilter);
        Task<List<Organization>> List(List<long> Ids);
        Task<Organization> Get(long Id);
        Task<List<long>> BulkMerge(List<Organization> Organizations);
    }
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DataContext DataContext;
        public OrganizationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<OrganizationDAO>> DynamicFilter(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.TaxCode, filter.TaxCode);
            if (filter.ParentId != null && filter.ParentId.HasValue)
            {
                if (filter.ParentId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.ParentId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Parent.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.ParentId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.ParentId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Parent.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.ParentId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.ParentId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.ParentId, IdFilter);
                }
                if (filter.ParentId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.ParentId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.ParentId, IdFilter);
                }
            }
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(OrganizationSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(OrganizationSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<OrganizationDAO>> OrFilter(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<OrganizationDAO> initQuery = query.Where(q => false);
            foreach (OrganizationFilter OrganizationFilter in filter.OrFilter)
            {
                IQueryable<OrganizationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, OrganizationFilter.Id);
                queryable = queryable.Where(q => q.Code, OrganizationFilter.Code);
                queryable = queryable.Where(q => q.Name, OrganizationFilter.Name);
                queryable = queryable.Where(q => q.Path, OrganizationFilter.Path);
                queryable = queryable.Where(q => q.Level, OrganizationFilter.Level);
                queryable = queryable.Where(q => q.Phone, OrganizationFilter.Phone);
                queryable = queryable.Where(q => q.Email, OrganizationFilter.Email);
                queryable = queryable.Where(q => q.Address, OrganizationFilter.Address);
                queryable = queryable.Where(q => q.TaxCode, OrganizationFilter.TaxCode);
                if (OrganizationFilter.ParentId != null && OrganizationFilter.ParentId.HasValue)
                {
                    if (OrganizationFilter.ParentId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.ParentId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Parent.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.ParentId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.ParentId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Parent.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.ParentId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.ParentId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.ParentId, IdFilter);
                    }
                    if (OrganizationFilter.ParentId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.ParentId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.ParentId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.StatusId, OrganizationFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<OrganizationDAO> DynamicOrder(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case OrganizationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case OrganizationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case OrganizationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case OrganizationOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case OrganizationOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case OrganizationOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case OrganizationOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case OrganizationOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case OrganizationOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                        case OrganizationOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case OrganizationOrder.TaxCode:
                            query = query.OrderBy(q => q.TaxCode);
                            break;
                        case OrganizationOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case OrganizationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case OrganizationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case OrganizationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case OrganizationOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case OrganizationOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case OrganizationOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case OrganizationOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case OrganizationOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case OrganizationOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                        case OrganizationOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case OrganizationOrder.TaxCode:
                            query = query.OrderByDescending(q => q.TaxCode);
                            break;
                        case OrganizationOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Organization>> DynamicSelect(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            List<Organization> Organizations = await query.Select(q => new Organization()
            {
                Id = filter.Selects.Contains(OrganizationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(OrganizationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(OrganizationSelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(OrganizationSelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(OrganizationSelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(OrganizationSelect.Level) ? q.Level : default(long),
                StatusId = filter.Selects.Contains(OrganizationSelect.Status) ? q.StatusId : default(long),
                Phone = filter.Selects.Contains(OrganizationSelect.Phone) ? q.Phone : default(string),
                Email = filter.Selects.Contains(OrganizationSelect.Email) ? q.Email : default(string),
                Address = filter.Selects.Contains(OrganizationSelect.Address) ? q.Address : default(string),
                TaxCode = filter.Selects.Contains(OrganizationSelect.TaxCode) ? q.TaxCode : default(string),
                Used = filter.Selects.Contains(OrganizationSelect.Used) ? q.Used : default(bool),
                Parent = filter.Selects.Contains(OrganizationSelect.Parent) && q.Parent != null ? new Organization
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Level = q.Parent.Level,
                    StatusId = q.Parent.StatusId,
                    Phone = q.Parent.Phone,
                    Email = q.Parent.Email,
                    Address = q.Parent.Address,
                    TaxCode = q.Parent.TaxCode,
                    Used = q.Parent.Used,
                } : null,
                Status = filter.Selects.Contains(OrganizationSelect.Status) && q.Status != null ? new Status
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
            return Organizations;
        }

        public async Task<int> CountAll(OrganizationFilter filter)
        {
            IQueryable<OrganizationDAO> OrganizationDAOs = DataContext.Organization.AsNoTracking();
            OrganizationDAOs = await DynamicFilter(OrganizationDAOs, filter);
            return await OrganizationDAOs.CountAsync();
        }

        public async Task<int> Count(OrganizationFilter filter)
        {
            IQueryable<OrganizationDAO> OrganizationDAOs = DataContext.Organization.AsNoTracking();
            OrganizationDAOs = await DynamicFilter(OrganizationDAOs, filter);
            OrganizationDAOs = await OrFilter(OrganizationDAOs, filter);
            return await OrganizationDAOs.CountAsync();
        }

        public async Task<List<Organization>> List(OrganizationFilter filter)
        {
            if (filter == null) return new List<Organization>();
            IQueryable<OrganizationDAO> OrganizationDAOs = DataContext.Organization.AsNoTracking();
            OrganizationDAOs = await DynamicFilter(OrganizationDAOs, filter);
            OrganizationDAOs = await OrFilter(OrganizationDAOs, filter);
            OrganizationDAOs = DynamicOrder(OrganizationDAOs, filter);
            List<Organization> Organizations = await DynamicSelect(OrganizationDAOs, filter);
            return Organizations;
        }

        public async Task<List<Organization>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<OrganizationDAO> query = DataContext.Organization.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Organization> Organizations = await query.AsNoTracking()
            .Select(x => new Organization()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                StatusId = x.StatusId,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                TaxCode = x.TaxCode,
                Used = x.Used,
                Parent = x.Parent == null ? null : new Organization
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    Phone = x.Parent.Phone,
                    Email = x.Parent.Email,
                    Address = x.Parent.Address,
                    TaxCode = x.Parent.TaxCode,
                    Used = x.Parent.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return Organizations;
        }

        public async Task<Organization> Get(long Id)
        {
            Organization Organization = await DataContext.Organization.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Organization()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                StatusId = x.StatusId,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                TaxCode = x.TaxCode,
                Used = x.Used,
                Parent = x.Parent == null ? null : new Organization
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    Phone = x.Parent.Phone,
                    Email = x.Parent.Email,
                    Address = x.Parent.Address,
                    TaxCode = x.Parent.TaxCode,
                    Used = x.Parent.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (Organization == null)
                return null;

            return Organization;
        }

        public async Task<List<long>> BulkMerge(List<Organization> Organizations)
        {
            IdFilter IdFilter = new IdFilter { In = Organizations.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<OrganizationDAO> OrganizationDAOs = new List<OrganizationDAO>();
            foreach (Organization Organization in Organizations)
            {
                OrganizationDAO OrganizationDAO = new OrganizationDAO();
                OrganizationDAOs.Add(OrganizationDAO);
                OrganizationDAO.Id = Organization.Id;
                OrganizationDAO.Code = Organization.Code;
                OrganizationDAO.Name = Organization.Name;
                OrganizationDAO.ParentId = Organization.ParentId;
                OrganizationDAO.Path = Organization.Path;
                OrganizationDAO.Level = Organization.Level;
                OrganizationDAO.StatusId = Organization.StatusId;
                OrganizationDAO.Phone = Organization.Phone;
                OrganizationDAO.Email = Organization.Email;
                OrganizationDAO.Address = Organization.Address;
                OrganizationDAO.TaxCode = Organization.TaxCode;
                OrganizationDAO.Used = Organization.Used;
                OrganizationDAO.CreatedAt = Organization.CreatedAt;
                OrganizationDAO.UpdatedAt = Organization.UpdatedAt;
                OrganizationDAO.DeletedAt = Organization.DeletedAt;
                OrganizationDAO.RowId = Organization.RowId;
            }
            await DataContext.BulkMergeAsync(OrganizationDAOs);
            var Ids = OrganizationDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        private async Task BuildPath()
        {
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
            Queue<OrganizationDAO> queue = new Queue<OrganizationDAO>();
            OrganizationDAOs.ForEach(x =>
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
                OrganizationDAO Parent = queue.Dequeue();
                foreach (OrganizationDAO OrganizationDAO in OrganizationDAOs)
                {
                    if (OrganizationDAO.ParentId == Parent.Id)
                    {
                        Parent.HasChildren = true;
                        OrganizationDAO.Path = Parent.Path + OrganizationDAO.Id + ".";
                        OrganizationDAO.Level = Parent.Level + 1;
                        queue.Enqueue(OrganizationDAO);
                    }
                }
            }
            await DataContext.SaveChangesAsync();
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Organization
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new OrganizationDAO { Used = true });
            return true;
        }
    }
}
