using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class ProductDAO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScanCode { get; set; }
        public string ERPCode { get; set; }
        public long CategoryId { get; set; }
        public long ProductTypeId { get; set; }
        public long? BrandId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? CodeGeneratorRuleId { get; set; }
        public long? UnitOfMeasureGroupingId { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public long TaxTypeId { get; set; }
        public long StatusId { get; set; }
        public string OtherName { get; set; }
        public string TechnicalName { get; set; }
        public string Note { get; set; }
        public bool IsPurchasable { get; set; }
        public bool IsSellable { get; set; }
        public bool IsNew { get; set; }
        public long UsedVariationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public bool IsCopyRightedProduct { get; set; }
        public bool IsBrandProduct { get; set; }

        public virtual BrandDAO Brand { get; set; }
        public virtual CategoryDAO Category { get; set; }
        public virtual ProductTypeDAO ProductType { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual TaxTypeDAO TaxType { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
        public virtual UnitOfMeasureGroupingDAO UnitOfMeasureGrouping { get; set; }
    }
}
