using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using IWM.Models;
using System;
using System.Threading.Tasks;
using TrueSight.Caching;
using TrueSight.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace IWM.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        Task Begin();
        Task Commit();
        Task Rollback();

        IAppUserRepository AppUserRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IImageRepository ImageRepository { get; }
        INationRepository NationRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IProvinceGroupingRepository ProvinceGroupingRepository { get; }
        ISexRepository SexRepository { get; }
        IStatusRepository StatusRepository { get; }
        ITaxTypeRepository TaxTypeRepository { get; }
        IUnitOfMeasureRepository UnitOfMeasureRepository { get; }
        IUnitOfMeasureGroupingContentRepository UnitOfMeasureGroupingContentRepository { get; }
        IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; }
        IWardRepository WardRepository { get; }
        IWorkerRepository WorkerRepository { get; }
        IWorkerGroupRepository WorkerGroupRepository { get; }
    }

    public class UOW : IUOW
    {
        private DataContext DataContext;
        private IDbContextTransaction TransactionScope;

        public IAppUserRepository AppUserRepository { get; private set; }
        public IBrandRepository BrandRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IDistrictRepository DistrictRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public INationRepository NationRepository { get; private set; }
        public IOrganizationRepository OrganizationRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public IProductTypeRepository ProductTypeRepository { get; private set; }
        public IProvinceRepository ProvinceRepository { get; private set; }
        public IProvinceGroupingRepository ProvinceGroupingRepository { get; private set; }
        public ISexRepository SexRepository { get; private set; }
        public IStatusRepository StatusRepository { get; private set; }
        public ITaxTypeRepository TaxTypeRepository { get; private set; }
        public IUnitOfMeasureRepository UnitOfMeasureRepository { get; private set; }
        public IUnitOfMeasureGroupingContentRepository UnitOfMeasureGroupingContentRepository { get; private set; }
        public IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; private set; }
        public IWardRepository WardRepository { get; private set; }
        public IWorkerRepository WorkerRepository { get; private set; }
        public IWorkerGroupRepository WorkerGroupRepository { get; private set; }

        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;

            AppUserRepository = new AppUserRepository(DataContext);
            BrandRepository = new BrandRepository(DataContext);
            CategoryRepository = new CategoryRepository(DataContext);
            DistrictRepository = new DistrictRepository(DataContext);
            ImageRepository = new ImageRepository(DataContext);
            NationRepository = new NationRepository(DataContext);
            OrganizationRepository = new OrganizationRepository(DataContext);
            ProductRepository = new ProductRepository(DataContext);
            ProductTypeRepository = new ProductTypeRepository(DataContext);
            ProvinceRepository = new ProvinceRepository(DataContext);
            ProvinceGroupingRepository = new ProvinceGroupingRepository(DataContext);
            SexRepository = new SexRepository(DataContext);
            StatusRepository = new StatusRepository(DataContext);
            TaxTypeRepository = new TaxTypeRepository(DataContext);
            UnitOfMeasureRepository = new UnitOfMeasureRepository(DataContext);
            UnitOfMeasureGroupingContentRepository = new UnitOfMeasureGroupingContentRepository(DataContext);
            UnitOfMeasureGroupingRepository = new UnitOfMeasureGroupingRepository(DataContext);
            WardRepository = new WardRepository(DataContext);
            WorkerRepository = new WorkerRepository(DataContext);
            WorkerGroupRepository = new WorkerGroupRepository(DataContext);
        }
        public async Task Begin()
        {
            TransactionScope = await DataContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            TransactionScope.Commit();
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            TransactionScope.Rollback();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }

            this.DataContext.Dispose();
            this.DataContext = null;
        }
    }
}