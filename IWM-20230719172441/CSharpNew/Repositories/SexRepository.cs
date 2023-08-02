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
    public interface ISexRepository
    {
        Task<int> CountAll(SexFilter SexFilter);
        Task<int> Count(SexFilter SexFilter);
        Task<List<Sex>> List(SexFilter SexFilter);
        Task<List<Sex>> List(List<long> Ids);
        Task<bool> BulkMerge(List<Sex> Sexes);
    }
    public class SexRepository : ISexRepository
    {
        private readonly DataContext DataContext;
        public SexRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<SexDAO>> DynamicFilter(IQueryable<SexDAO> query, SexFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);

            return query;
        }

        private async Task<IQueryable<SexDAO>> OrFilter(IQueryable<SexDAO> query, SexFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            List<IQueryable<long>> Queries = new List<IQueryable<long>>();
            foreach (SexFilter SexFilter in filter.OrFilter)
            {
                IQueryable<SexDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, SexFilter.Id);
                queryable = queryable.Where(q => q.Code, SexFilter.Code);
                queryable = queryable.Where(q => q.Name, SexFilter.Name);
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

        private IQueryable<SexDAO> DynamicOrder(IQueryable<SexDAO> query, SexFilter filter)
        {
            Dictionary<SexOrder, LambdaExpression> CustomOrder = new Dictionary<SexOrder, LambdaExpression>()
            {
            };
            query = query.OrderBy(filter.OrderBy, filter.OrderType, CustomOrder);
            query = query.Paging(filter);
            return query;
        }

        private async Task<List<Sex>> DynamicSelect(IQueryable<SexDAO> query, SexFilter filter)
        {
            List<Sex> Sexes = await query.Select(q => new Sex()
            {
                Id = filter.Selects.Contains(SexSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(SexSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(SexSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return Sexes;
        }

        public async Task<int> CountAll(SexFilter filter)
        {
            IQueryable<SexDAO> SexDAOs = DataContext.Sex.AsNoTracking();
            SexDAOs = await DynamicFilter(SexDAOs, filter);
            return await SexDAOs.CountAsync();
        }

        public async Task<int> Count(SexFilter filter)
        {
            IQueryable<SexDAO> SexDAOs = DataContext.Sex.AsNoTracking();
            SexDAOs = await DynamicFilter(SexDAOs, filter);
            SexDAOs = await OrFilter(SexDAOs, filter);
            return await SexDAOs.CountAsync();
        }

        public async Task<List<Sex>> List(SexFilter filter)
        {
            if (filter == null) return new List<Sex>();
            IQueryable<SexDAO> SexDAOs = DataContext.Sex.AsNoTracking();
            SexDAOs = await DynamicFilter(SexDAOs, filter);
            SexDAOs = await OrFilter(SexDAOs, filter);
            SexDAOs = DynamicOrder(SexDAOs, filter);
            List<Sex> Sexes = await DynamicSelect(SexDAOs, filter);
            return Sexes;
        }

        public async Task<List<Sex>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<SexDAO> query = DataContext.Sex.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<Sex> Sexes = await query.AsNoTracking()
            .Select(x => new Sex()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListAsync();


            return Sexes;
        }

        public async Task<bool> BulkMerge(List<Sex> Sexes)
        {
            List<SexDAO> SexDAOs = new List<SexDAO>();
            foreach (var Sex in Sexes)
            {
                SexDAO SexDAO = new SexDAO();
                SexDAO.Id = Sex.Id;
                SexDAO.Code = Sex.Code;
                SexDAO.Name = Sex.Name;
                SexDAOs.Add(SexDAO);
            }
            await DataContext.Sex.BulkMergeAsync(SexDAOs);
            return true;
        }
    }
}
