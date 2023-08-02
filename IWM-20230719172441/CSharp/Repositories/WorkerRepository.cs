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
    public interface IWorkerRepository
    {
        Task<int> CountAll(WorkerFilter WorkerFilter);
        Task<int> Count(WorkerFilter WorkerFilter);
        Task<List<Worker>> List(WorkerFilter WorkerFilter);
        Task<List<Worker>> List(List<long> Ids);
        Task<Worker> Get(long Id);
        Task<bool> Create(Worker Worker);
        Task<bool> Update(Worker Worker);
        Task<bool> Delete(Worker Worker);
        Task<List<long>> BulkMerge(List<Worker> Workers);
        Task<bool> BulkDelete(List<Worker> Workers);
    }
    public class WorkerRepository : IWorkerRepository
    {
        private readonly DataContext DataContext;
        public WorkerRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<WorkerDAO>> DynamicFilter(IQueryable<WorkerDAO> query, WorkerFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Birthday, filter.Birthday);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.CitizenIdentificationNumber, filter.CitizenIdentificationNumber);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.Username, filter.Username);
            query = query.Where(q => q.Password, filter.Password);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.NationId, filter.NationId);
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.SexId, filter.SexId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.WardId, filter.WardId);
            query = query.Where(q => q.WorkerGroupId, filter.WorkerGroupId);
            if (filter.Search != null)
            {
                 query = query.Where(q => 
                    (filter.SearchBy.Contains(WorkerSearch.Code) && q.Code.ToLower().Contains(filter.Search.ToLower())) ||
                    (filter.SearchBy.Contains(WorkerSearch.Name) && q.Name.ToLower().Contains(filter.Search.ToLower())));
            }

            return query;
        }

        private async Task<IQueryable<WorkerDAO>> OrFilter(IQueryable<WorkerDAO> query, WorkerFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkerDAO> initQuery = query.Where(q => false);
            foreach (WorkerFilter WorkerFilter in filter.OrFilter)
            {
                IQueryable<WorkerDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkerFilter.Id);
                queryable = queryable.Where(q => q.Code, WorkerFilter.Code);
                queryable = queryable.Where(q => q.Name, WorkerFilter.Name);
                queryable = queryable.Where(q => q.Birthday, WorkerFilter.Birthday);
                queryable = queryable.Where(q => q.Phone, WorkerFilter.Phone);
                queryable = queryable.Where(q => q.CitizenIdentificationNumber, WorkerFilter.CitizenIdentificationNumber);
                queryable = queryable.Where(q => q.Email, WorkerFilter.Email);
                queryable = queryable.Where(q => q.Address, WorkerFilter.Address);
                queryable = queryable.Where(q => q.Username, WorkerFilter.Username);
                queryable = queryable.Where(q => q.Password, WorkerFilter.Password);
                queryable = queryable.Where(q => q.DistrictId, WorkerFilter.DistrictId);
                queryable = queryable.Where(q => q.NationId, WorkerFilter.NationId);
                queryable = queryable.Where(q => q.ProvinceId, WorkerFilter.ProvinceId);
                queryable = queryable.Where(q => q.SexId, WorkerFilter.SexId);
                queryable = queryable.Where(q => q.StatusId, WorkerFilter.StatusId);
                queryable = queryable.Where(q => q.WardId, WorkerFilter.WardId);
                queryable = queryable.Where(q => q.WorkerGroupId, WorkerFilter.WorkerGroupId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<WorkerDAO> DynamicOrder(IQueryable<WorkerDAO> query, WorkerFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkerOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkerOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WorkerOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WorkerOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case WorkerOrder.Birthday:
                            query = query.OrderBy(q => q.Birthday);
                            break;
                        case WorkerOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case WorkerOrder.CitizenIdentificationNumber:
                            query = query.OrderBy(q => q.CitizenIdentificationNumber);
                            break;
                        case WorkerOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                        case WorkerOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case WorkerOrder.Sex:
                            query = query.OrderBy(q => q.SexId);
                            break;
                        case WorkerOrder.WorkerGroup:
                            query = query.OrderBy(q => q.WorkerGroupId);
                            break;
                        case WorkerOrder.Nation:
                            query = query.OrderBy(q => q.NationId);
                            break;
                        case WorkerOrder.Province:
                            query = query.OrderBy(q => q.ProvinceId);
                            break;
                        case WorkerOrder.District:
                            query = query.OrderBy(q => q.DistrictId);
                            break;
                        case WorkerOrder.Ward:
                            query = query.OrderBy(q => q.WardId);
                            break;
                        case WorkerOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case WorkerOrder.Password:
                            query = query.OrderBy(q => q.Password);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkerOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkerOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WorkerOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WorkerOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case WorkerOrder.Birthday:
                            query = query.OrderByDescending(q => q.Birthday);
                            break;
                        case WorkerOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case WorkerOrder.CitizenIdentificationNumber:
                            query = query.OrderByDescending(q => q.CitizenIdentificationNumber);
                            break;
                        case WorkerOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                        case WorkerOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case WorkerOrder.Sex:
                            query = query.OrderByDescending(q => q.SexId);
                            break;
                        case WorkerOrder.WorkerGroup:
                            query = query.OrderByDescending(q => q.WorkerGroupId);
                            break;
                        case WorkerOrder.Nation:
                            query = query.OrderByDescending(q => q.NationId);
                            break;
                        case WorkerOrder.Province:
                            query = query.OrderByDescending(q => q.ProvinceId);
                            break;
                        case WorkerOrder.District:
                            query = query.OrderByDescending(q => q.DistrictId);
                            break;
                        case WorkerOrder.Ward:
                            query = query.OrderByDescending(q => q.WardId);
                            break;
                        case WorkerOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case WorkerOrder.Password:
                            query = query.OrderByDescending(q => q.Password);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Worker>> DynamicSelect(IQueryable<WorkerDAO> query, WorkerFilter filter)
        {
            List<Worker> Workers = await query.Select(q => new Worker()
            {
                Id = filter.Selects.Contains(WorkerSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WorkerSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WorkerSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(WorkerSelect.Status) ? q.StatusId : default(long),
                Birthday = filter.Selects.Contains(WorkerSelect.Birthday) ? q.Birthday : default(DateTime?),
                Phone = filter.Selects.Contains(WorkerSelect.Phone) ? q.Phone : default(string),
                CitizenIdentificationNumber = filter.Selects.Contains(WorkerSelect.CitizenIdentificationNumber) ? q.CitizenIdentificationNumber : default(string),
                Email = filter.Selects.Contains(WorkerSelect.Email) ? q.Email : default(string),
                Address = filter.Selects.Contains(WorkerSelect.Address) ? q.Address : default(string),
                SexId = filter.Selects.Contains(WorkerSelect.Sex) ? q.SexId : default(long?),
                WorkerGroupId = filter.Selects.Contains(WorkerSelect.WorkerGroup) ? q.WorkerGroupId : default(long?),
                NationId = filter.Selects.Contains(WorkerSelect.Nation) ? q.NationId : default(long?),
                ProvinceId = filter.Selects.Contains(WorkerSelect.Province) ? q.ProvinceId : default(long?),
                DistrictId = filter.Selects.Contains(WorkerSelect.District) ? q.DistrictId : default(long?),
                WardId = filter.Selects.Contains(WorkerSelect.Ward) ? q.WardId : default(long?),
                Username = filter.Selects.Contains(WorkerSelect.Username) ? q.Username : default(string),
                Password = filter.Selects.Contains(WorkerSelect.Password) ? q.Password : default(string),
                District = filter.Selects.Contains(WorkerSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                    Priority = q.District.Priority,
                    ProvinceId = q.District.ProvinceId,
                    StatusId = q.District.StatusId,
                    Used = q.District.Used,
                } : null,
                Nation = filter.Selects.Contains(WorkerSelect.Nation) && q.Nation != null ? new Nation
                {
                    Id = q.Nation.Id,
                    Code = q.Nation.Code,
                    Name = q.Nation.Name,
                    Priority = q.Nation.Priority,
                    StatusId = q.Nation.StatusId,
                    Used = q.Nation.Used,
                } : null,
                Province = filter.Selects.Contains(WorkerSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                    Priority = q.Province.Priority,
                    StatusId = q.Province.StatusId,
                    Used = q.Province.Used,
                } : null,
                Sex = filter.Selects.Contains(WorkerSelect.Sex) && q.Sex != null ? new Sex
                {
                    Id = q.Sex.Id,
                    Code = q.Sex.Code,
                    Name = q.Sex.Name,
                } : null,
                Status = filter.Selects.Contains(WorkerSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                    Color = q.Status.Color,
                } : null,
                Ward = filter.Selects.Contains(WorkerSelect.Ward) && q.Ward != null ? new Ward
                {
                    Id = q.Ward.Id,
                    Code = q.Ward.Code,
                    Name = q.Ward.Name,
                    Priority = q.Ward.Priority,
                    DistrictId = q.Ward.DistrictId,
                    StatusId = q.Ward.StatusId,
                    Used = q.Ward.Used,
                } : null,
                WorkerGroup = filter.Selects.Contains(WorkerSelect.WorkerGroup) && q.WorkerGroup != null ? new WorkerGroup
                {
                    Id = q.WorkerGroup.Id,
                    Code = q.WorkerGroup.Code,
                    Name = q.WorkerGroup.Name,
                    StatusId = q.WorkerGroup.StatusId,
                } : null,
            }).ToListAsync();
            return Workers;
        }

        public async Task<int> CountAll(WorkerFilter filter)
        {
            IQueryable<WorkerDAO> WorkerDAOs = DataContext.Worker.AsNoTracking();
            WorkerDAOs = await DynamicFilter(WorkerDAOs, filter);
            return await WorkerDAOs.CountAsync();
        }

        public async Task<int> Count(WorkerFilter filter)
        {
            IQueryable<WorkerDAO> WorkerDAOs = DataContext.Worker.AsNoTracking();
            WorkerDAOs = await DynamicFilter(WorkerDAOs, filter);
            WorkerDAOs = await OrFilter(WorkerDAOs, filter);
            return await WorkerDAOs.CountAsync();
        }

        public async Task<List<Worker>> List(WorkerFilter filter)
        {
            if (filter == null) return new List<Worker>();
            IQueryable<WorkerDAO> WorkerDAOs = DataContext.Worker.AsNoTracking();
            WorkerDAOs = await DynamicFilter(WorkerDAOs, filter);
            WorkerDAOs = await OrFilter(WorkerDAOs, filter);
            WorkerDAOs = DynamicOrder(WorkerDAOs, filter);
            List<Worker> Workers = await DynamicSelect(WorkerDAOs, filter);
            return Workers;
        }

        public async Task<List<Worker>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<WorkerDAO> query = DataContext.Worker.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Worker> Workers = await query.AsNoTracking()
            .Select(x => new Worker()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Birthday = x.Birthday,
                Phone = x.Phone,
                CitizenIdentificationNumber = x.CitizenIdentificationNumber,
                Email = x.Email,
                Address = x.Address,
                SexId = x.SexId,
                WorkerGroupId = x.WorkerGroupId,
                NationId = x.NationId,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Username = x.Username,
                Password = x.Password,
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
                Nation = x.Nation == null ? null : new Nation
                {
                    Id = x.Nation.Id,
                    Code = x.Nation.Code,
                    Name = x.Nation.Name,
                    Priority = x.Nation.Priority,
                    StatusId = x.Nation.StatusId,
                    Used = x.Nation.Used,
                },
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                    Used = x.Province.Used,
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
                Ward = x.Ward == null ? null : new Ward
                {
                    Id = x.Ward.Id,
                    Code = x.Ward.Code,
                    Name = x.Ward.Name,
                    Priority = x.Ward.Priority,
                    DistrictId = x.Ward.DistrictId,
                    StatusId = x.Ward.StatusId,
                    Used = x.Ward.Used,
                },
                WorkerGroup = x.WorkerGroup == null ? null : new WorkerGroup
                {
                    Id = x.WorkerGroup.Id,
                    Code = x.WorkerGroup.Code,
                    Name = x.WorkerGroup.Name,
                    StatusId = x.WorkerGroup.StatusId,
                },
            }).ToListAsync();
            

            return Workers;
        }

        public async Task<Worker> Get(long Id)
        {
            Worker Worker = await DataContext.Worker.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new Worker()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Birthday = x.Birthday,
                Phone = x.Phone,
                CitizenIdentificationNumber = x.CitizenIdentificationNumber,
                Email = x.Email,
                Address = x.Address,
                SexId = x.SexId,
                WorkerGroupId = x.WorkerGroupId,
                NationId = x.NationId,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Username = x.Username,
                Password = x.Password,
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
                Nation = x.Nation == null ? null : new Nation
                {
                    Id = x.Nation.Id,
                    Code = x.Nation.Code,
                    Name = x.Nation.Name,
                    Priority = x.Nation.Priority,
                    StatusId = x.Nation.StatusId,
                    Used = x.Nation.Used,
                },
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                    Used = x.Province.Used,
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
                Ward = x.Ward == null ? null : new Ward
                {
                    Id = x.Ward.Id,
                    Code = x.Ward.Code,
                    Name = x.Ward.Name,
                    Priority = x.Ward.Priority,
                    DistrictId = x.Ward.DistrictId,
                    StatusId = x.Ward.StatusId,
                    Used = x.Ward.Used,
                },
                WorkerGroup = x.WorkerGroup == null ? null : new WorkerGroup
                {
                    Id = x.WorkerGroup.Id,
                    Code = x.WorkerGroup.Code,
                    Name = x.WorkerGroup.Name,
                    StatusId = x.WorkerGroup.StatusId,
                },
            }).FirstOrDefaultAsync();

            if (Worker == null)
                return null;

            return Worker;
        }
        public async Task<bool> Create(Worker Worker)
        {
            WorkerDAO WorkerDAO = new WorkerDAO();
            WorkerDAO.Id = Worker.Id;
            WorkerDAO.Code = Worker.Code;
            WorkerDAO.Name = Worker.Name;
            WorkerDAO.StatusId = Worker.StatusId;
            WorkerDAO.Birthday = Worker.Birthday;
            WorkerDAO.Phone = Worker.Phone;
            WorkerDAO.CitizenIdentificationNumber = Worker.CitizenIdentificationNumber;
            WorkerDAO.Email = Worker.Email;
            WorkerDAO.Address = Worker.Address;
            WorkerDAO.SexId = Worker.SexId;
            WorkerDAO.WorkerGroupId = Worker.WorkerGroupId;
            WorkerDAO.NationId = Worker.NationId;
            WorkerDAO.ProvinceId = Worker.ProvinceId;
            WorkerDAO.DistrictId = Worker.DistrictId;
            WorkerDAO.WardId = Worker.WardId;
            WorkerDAO.Username = Worker.Username;
            WorkerDAO.Password = Worker.Password;
            DataContext.Worker.Add(WorkerDAO);
            await DataContext.SaveChangesAsync();
            Worker.Id = WorkerDAO.Id;
            await SaveReference(Worker);
            return true;
        }

        public async Task<bool> Update(Worker Worker)
        {
            WorkerDAO WorkerDAO = DataContext.Worker
                .Where(x => x.Id == Worker.Id)
                .FirstOrDefault();
            if (WorkerDAO == null)
                return false;
            WorkerDAO.Id = Worker.Id;
            WorkerDAO.Code = Worker.Code;
            WorkerDAO.Name = Worker.Name;
            WorkerDAO.StatusId = Worker.StatusId;
            WorkerDAO.Birthday = Worker.Birthday;
            WorkerDAO.Phone = Worker.Phone;
            WorkerDAO.CitizenIdentificationNumber = Worker.CitizenIdentificationNumber;
            WorkerDAO.Email = Worker.Email;
            WorkerDAO.Address = Worker.Address;
            WorkerDAO.SexId = Worker.SexId;
            WorkerDAO.WorkerGroupId = Worker.WorkerGroupId;
            WorkerDAO.NationId = Worker.NationId;
            WorkerDAO.ProvinceId = Worker.ProvinceId;
            WorkerDAO.DistrictId = Worker.DistrictId;
            WorkerDAO.WardId = Worker.WardId;
            WorkerDAO.Username = Worker.Username;
            WorkerDAO.Password = Worker.Password;
            await DataContext.SaveChangesAsync();
            await SaveReference(Worker);
            return true;
        }

        public async Task<bool> Delete(Worker Worker)
        {
            await DataContext.Worker
                .Where(x => x.Id == Worker.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<List<long>> BulkMerge(List<Worker> Workers)
        {
            IdFilter IdFilter = new IdFilter { In = Workers.Where(x => x.Id != 0).Select(x => x.Id).ToList() };
            List<WorkerDAO> WorkerDAOs = new List<WorkerDAO>();
            foreach (Worker Worker in Workers)
            {
                WorkerDAO WorkerDAO = new WorkerDAO();
                WorkerDAOs.Add(WorkerDAO);
                WorkerDAO.Id = Worker.Id;
                WorkerDAO.Code = Worker.Code;
                WorkerDAO.Name = Worker.Name;
                WorkerDAO.StatusId = Worker.StatusId;
                WorkerDAO.Birthday = Worker.Birthday;
                WorkerDAO.Phone = Worker.Phone;
                WorkerDAO.CitizenIdentificationNumber = Worker.CitizenIdentificationNumber;
                WorkerDAO.Email = Worker.Email;
                WorkerDAO.Address = Worker.Address;
                WorkerDAO.SexId = Worker.SexId;
                WorkerDAO.WorkerGroupId = Worker.WorkerGroupId;
                WorkerDAO.NationId = Worker.NationId;
                WorkerDAO.ProvinceId = Worker.ProvinceId;
                WorkerDAO.DistrictId = Worker.DistrictId;
                WorkerDAO.WardId = Worker.WardId;
                WorkerDAO.Username = Worker.Username;
                WorkerDAO.Password = Worker.Password;
            }
            await DataContext.BulkMergeAsync(WorkerDAOs);
            var Ids = WorkerDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
        
        public async Task<bool> BulkDelete(List<Worker> Workers)
        {
            List<long> Ids = Workers.Select(x => x.Id).ToList();
            await DataContext.Worker
                .WhereBulkContains(Ids, x => x.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Worker Worker)
        {
        }

    }
}
