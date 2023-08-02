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
    public interface IAppUserRepository
    {
        Task<int> CountAll(AppUserFilter AppUserFilter);
        Task<int> Count(AppUserFilter AppUserFilter);
        Task<List<AppUser>> List(AppUserFilter AppUserFilter);
        Task<List<AppUser>> List(List<long> Ids);
        Task<AppUser> Get(long Id);
        Task<List<long>> BulkMerge(List<AppUser> AppUsers);
    }
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext DataContext;
        public AppUserRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<AppUserDAO>> DynamicFilter(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Username, filter.Username);
            query = query.Where(q => q.DisplayName, filter.DisplayName);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.Birthday, filter.Birthday);
            query = query.Where(q => q.Avatar, filter.Avatar);
            query = query.Where(q => q.Department, filter.Department);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            if (filter.OrganizationId != null && filter.OrganizationId.HasValue)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.SexId, filter.SexId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(AppUserSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(AppUserSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<AppUserDAO>> OrFilter(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<AppUserDAO> initQuery = query.Where(q => false);
            foreach (AppUserFilter AppUserFilter in filter.OrFilter)
            {
                IQueryable<AppUserDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, AppUserFilter.Id);
                queryable = queryable.Where(q => q.Username, AppUserFilter.Username);
                queryable = queryable.Where(q => q.DisplayName, AppUserFilter.DisplayName);
                queryable = queryable.Where(q => q.Address, AppUserFilter.Address);
                queryable = queryable.Where(q => q.Email, AppUserFilter.Email);
                queryable = queryable.Where(q => q.Phone, AppUserFilter.Phone);
                queryable = queryable.Where(q => q.Birthday, AppUserFilter.Birthday);
                queryable = queryable.Where(q => q.Avatar, AppUserFilter.Avatar);
                queryable = queryable.Where(q => q.Department, AppUserFilter.Department);
                queryable = queryable.Where(q => q.Code, AppUserFilter.Code);
                queryable = queryable.Where(q => q.Name, AppUserFilter.Name);
                if (AppUserFilter.OrganizationId != null && AppUserFilter.OrganizationId.HasValue)
                {
                    if (AppUserFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == AppUserFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (AppUserFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == AppUserFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (AppUserFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => AppUserFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                    if (AppUserFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => AppUserFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.SexId, AppUserFilter.SexId);
                queryable = queryable.Where(q => q.StatusId, AppUserFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<AppUserDAO> DynamicOrder(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case AppUserOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case AppUserOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case AppUserOrder.DisplayName:
                            query = query.OrderBy(q => q.DisplayName);
                            break;
                        case AppUserOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case AppUserOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                        case AppUserOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case AppUserOrder.Sex:
                            query = query.OrderBy(q => q.SexId);
                            break;
                        case AppUserOrder.Birthday:
                            query = query.OrderBy(q => q.Birthday);
                            break;
                        case AppUserOrder.Avatar:
                            query = query.OrderBy(q => q.Avatar);
                            break;
                        case AppUserOrder.Department:
                            query = query.OrderBy(q => q.Department);
                            break;
                        case AppUserOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case AppUserOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case AppUserOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                        case AppUserOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case AppUserOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case AppUserOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case AppUserOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case AppUserOrder.DisplayName:
                            query = query.OrderByDescending(q => q.DisplayName);
                            break;
                        case AppUserOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case AppUserOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                        case AppUserOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case AppUserOrder.Sex:
                            query = query.OrderByDescending(q => q.SexId);
                            break;
                        case AppUserOrder.Birthday:
                            query = query.OrderByDescending(q => q.Birthday);
                            break;
                        case AppUserOrder.Avatar:
                            query = query.OrderByDescending(q => q.Avatar);
                            break;
                        case AppUserOrder.Department:
                            query = query.OrderByDescending(q => q.Department);
                            break;
                        case AppUserOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case AppUserOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case AppUserOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                        case AppUserOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case AppUserOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<AppUser>> DynamicSelect(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            List<AppUser> AppUsers = await query.Select(q => new AppUser()
            {
                Id = filter.Selects.Contains(AppUserSelect.Id) ? q.Id : default(long),
                Username = filter.Selects.Contains(AppUserSelect.Username) ? q.Username : default(string),
                DisplayName = filter.Selects.Contains(AppUserSelect.DisplayName) ? q.DisplayName : default(string),
                Address = filter.Selects.Contains(AppUserSelect.Address) ? q.Address : default(string),
                Email = filter.Selects.Contains(AppUserSelect.Email) ? q.Email : default(string),
                Phone = filter.Selects.Contains(AppUserSelect.Phone) ? q.Phone : default(string),
                SexId = filter.Selects.Contains(AppUserSelect.Sex) ? q.SexId : default(long),
                Birthday = filter.Selects.Contains(AppUserSelect.Birthday) ? q.Birthday : default(DateTime?),
                Avatar = filter.Selects.Contains(AppUserSelect.Avatar) ? q.Avatar : default(string),
                Department = filter.Selects.Contains(AppUserSelect.Department) ? q.Department : default(string),
                OrganizationId = filter.Selects.Contains(AppUserSelect.Organization) ? q.OrganizationId : default(long),
                StatusId = filter.Selects.Contains(AppUserSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(AppUserSelect.Used) ? q.Used : default(bool),
                Code = filter.Selects.Contains(AppUserSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(AppUserSelect.Name) ? q.Name : default(string),
                Organization = filter.Selects.Contains(AppUserSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                    TaxCode = q.Organization.TaxCode,
                    Used = q.Organization.Used,
                } : null,
                Sex = filter.Selects.Contains(AppUserSelect.Sex) && q.Sex != null ? new Sex
                {
                    Id = q.Sex.Id,
                    Code = q.Sex.Code,
                    Name = q.Sex.Name,
                } : null,
                Status = filter.Selects.Contains(AppUserSelect.Status) && q.Status != null ? new Status
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
            return AppUsers;
        }

        public async Task<int> CountAll(AppUserFilter filter)
        {
            IQueryable<AppUserDAO> AppUserDAOs = DataContext.AppUser.AsNoTracking();
            AppUserDAOs = await DynamicFilter(AppUserDAOs, filter);
            return await AppUserDAOs.CountAsync();
        }

        public async Task<int> Count(AppUserFilter filter)
        {
            IQueryable<AppUserDAO> AppUserDAOs = DataContext.AppUser.AsNoTracking();
            AppUserDAOs = await DynamicFilter(AppUserDAOs, filter);
            AppUserDAOs = await OrFilter(AppUserDAOs, filter);
            return await AppUserDAOs.CountAsync();
        }

        public async Task<List<AppUser>> List(AppUserFilter filter)
        {
            if (filter == null) return new List<AppUser>();
            IQueryable<AppUserDAO> AppUserDAOs = DataContext.AppUser.AsNoTracking();
            AppUserDAOs = await DynamicFilter(AppUserDAOs, filter);
            AppUserDAOs = await OrFilter(AppUserDAOs, filter);
            AppUserDAOs = DynamicOrder(AppUserDAOs, filter);
            List<AppUser> AppUsers = await DynamicSelect(AppUserDAOs, filter);
            return AppUsers;
        }

        public async Task<List<AppUser>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<AppUserDAO> query = DataContext.AppUser.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<AppUser> AppUsers = await query.AsNoTracking()
            .Select(x => new AppUser()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Address = x.Address,
                Email = x.Email,
                Phone = x.Phone,
                SexId = x.SexId,
                Birthday = x.Birthday,
                Avatar = x.Avatar,
                Department = x.Department,
                OrganizationId = x.OrganizationId,
                StatusId = x.StatusId,
                Used = x.Used,
                Code = x.Code,
                Name = x.Name,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    TaxCode = x.Organization.TaxCode,
                    Used = x.Organization.Used,
                },
                Sex = x.Sex == null ? null : new Sex
                {
                    Id = x.Sex.Id,
                    Code = x.Sex.Code,
                    Name = x.Sex.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).ToListAsync();
            

            return AppUsers;
        }

        public async Task<AppUser> Get(long Id)
        {
            AppUser AppUser = await DataContext.AppUser.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new AppUser()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Address = x.Address,
                Email = x.Email,
                Phone = x.Phone,
                SexId = x.SexId,
                Birthday = x.Birthday,
                Avatar = x.Avatar,
                Department = x.Department,
                OrganizationId = x.OrganizationId,
                StatusId = x.StatusId,
                Used = x.Used,
                Code = x.Code,
                Name = x.Name,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    TaxCode = x.Organization.TaxCode,
                    Used = x.Organization.Used,
                },
                Sex = x.Sex == null ? null : new Sex
                {
                    Id = x.Sex.Id,
                    Code = x.Sex.Code,
                    Name = x.Sex.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                    Color = x.Status.Color,
                },
            }).FirstOrDefaultAsync();

            if (AppUser == null)
                return null;

            return AppUser;
        }

        public async Task<List<long>> BulkMerge(List<AppUser> AppUsers)
        {
            IdFilter IdFilter = new IdFilter { In = AppUsers.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<AppUserDAO> AppUserDAOs = new List<AppUserDAO>();
            foreach (AppUser AppUser in AppUsers)
            {
                AppUserDAO AppUserDAO = new AppUserDAO();
                AppUserDAOs.Add(AppUserDAO);
                AppUserDAO.Id = AppUser.Id;
                AppUserDAO.Username = AppUser.Username;
                AppUserDAO.DisplayName = AppUser.DisplayName;
                AppUserDAO.Address = AppUser.Address;
                AppUserDAO.Email = AppUser.Email;
                AppUserDAO.Phone = AppUser.Phone;
                AppUserDAO.SexId = AppUser.SexId;
                AppUserDAO.Birthday = AppUser.Birthday;
                AppUserDAO.Avatar = AppUser.Avatar;
                AppUserDAO.Department = AppUser.Department;
                AppUserDAO.OrganizationId = AppUser.OrganizationId;
                AppUserDAO.StatusId = AppUser.StatusId;
                AppUserDAO.Used = AppUser.Used;
                AppUserDAO.Code = AppUser.Code;
                AppUserDAO.Name = AppUser.Name;
                AppUserDAO.CreatedAt = AppUser.CreatedAt;
                AppUserDAO.UpdatedAt = AppUser.UpdatedAt;
                AppUserDAO.DeletedAt = AppUser.DeletedAt;
                AppUserDAO.RowId = AppUser.RowId;
            }
            await DataContext.BulkMergeAsync(AppUserDAOs);
            var Ids = AppUserDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.AppUser
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new AppUserDAO { Used = true });
            return true;
        }
    }
}
