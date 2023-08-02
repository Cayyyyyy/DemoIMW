using IWM.Entities;
using TrueSight.RabbitMQ;

namespace IWM.Handlers
{
    public class MessageRoutingKey : BaseMessageRoutingKey
    {
        public const string AppUserSync = nameof(AppUser) + BaseSyncData;
        public const string BrandSync = nameof(Brand) + BaseSyncData;
        public const string CategorySync = nameof(Category) + BaseSyncData;
        public const string DistrictSync = nameof(District) + BaseSyncData;
        public const string ImageSync = nameof(Image) + BaseSyncData;
        public const string NationSync = nameof(Nation) + BaseSyncData;
        public const string OrganizationSync = nameof(Organization) + BaseSyncData;
        public const string ProductSync = nameof(Product) + BaseSyncData;
        public const string ProductTypeSync = nameof(ProductType) + BaseSyncData;
        public const string ProvinceSync = nameof(Province) + BaseSyncData;
        public const string ProvinceGroupingSync = nameof(ProvinceGrouping) + BaseSyncData;
        public const string SexSync = nameof(Sex) + BaseSyncData;
        public const string StatusSync = nameof(Status) + BaseSyncData;
        public const string TaxTypeSync = nameof(TaxType) + BaseSyncData;
        public const string UnitOfMeasureSync = nameof(UnitOfMeasure) + BaseSyncData;
        public const string UnitOfMeasureGroupingSync = nameof(UnitOfMeasureGrouping) + BaseSyncData;
        public const string WardSync = nameof(Ward) + BaseSyncData;
        public const string WorkerSync = nameof(Worker) + BaseSyncData;
        public const string WorkerGroupSync = nameof(WorkerGroup) + BaseSyncData;

        public const string AppUserUsed = nameof(AppUser) + BaseUsedData;
        public const string BrandUsed = nameof(Brand) + BaseUsedData;
        public const string CategoryUsed = nameof(Category) + BaseUsedData;
        public const string DistrictUsed = nameof(District) + BaseUsedData;
        public const string ImageUsed = nameof(Image) + BaseUsedData;
        public const string NationUsed = nameof(Nation) + BaseUsedData;
        public const string OrganizationUsed = nameof(Organization) + BaseUsedData;
        public const string ProductUsed = nameof(Product) + BaseUsedData;
        public const string ProductTypeUsed = nameof(ProductType) + BaseUsedData;
        public const string ProvinceUsed = nameof(Province) + BaseUsedData;
        public const string ProvinceGroupingUsed = nameof(ProvinceGrouping) + BaseUsedData;
        public const string SexUsed = nameof(Sex) + BaseUsedData;
        public const string StatusUsed = nameof(Status) + BaseUsedData;
        public const string TaxTypeUsed = nameof(TaxType) + BaseUsedData;
        public const string UnitOfMeasureUsed = nameof(UnitOfMeasure) + BaseUsedData;
        public const string UnitOfMeasureGroupingUsed = nameof(UnitOfMeasureGrouping) + BaseUsedData;
        public const string WardUsed = nameof(Ward) + BaseUsedData;
        public const string WorkerUsed = nameof(Worker) + BaseUsedData;
        public const string WorkerGroupUsed = nameof(WorkerGroup) + BaseUsedData;
    }
}
