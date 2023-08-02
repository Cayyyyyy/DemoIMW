using IWM.Entities;

namespace IWM.Handlers
{
    public class MessageRoutingKey
    {
        //Common
        public const string MenuSend = "Menu.Send";
        public const string RoleSend = "Role.Send";
        public const string MasterEntityRegister = "MasterEntity.Register";
        public const string ApprovalTypeRegister = "ApprovalType.Register";

        public const string BaseSyncData = ".Sync";
        public const string BaseUsedData = ".Used";

        public const string ProvinceGroupingSync = nameof(ProvinceGrouping) + BaseSyncData;
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
