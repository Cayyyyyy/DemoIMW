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
            query = query.Where(q => q.OrganizationId, filter.OrganizationId, DataContext.Organization.AsNoTracking());
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
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
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
                queryable = queryable.Where(q => q.OrganizationId, AppUserFilter.OrganizationId, DataContext.Organization.AsNoTracking());
                queryable = queryable.Where(q => q.SexId, AppUserFilter.SexId);
                queryable = queryable.Where(q => q.StatusId, AppUserFilter.StatusId);
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

        private IQueryable<AppUserDAO> DynamicOrder(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            Dictionary<AppUserOrder, LambdaExpression> CustomOrder = new Dictionary<AppUserOrder, LambdaExpression>()
            {
                { AppUserOrder.Organization, (AppUserDAO x) => x.Organization.Name  },
                { AppUserOrder.Sex, (AppUserDAO x) => x.Sex.Name  },
                { AppUserOrder.Status, (AppUserDAO x) => x.Status.Name  },
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
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
