using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class Product : DataEntity
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
        public bool Used { get; set; }
        public bool IsCopyRightedProduct { get; set; }
        public bool IsBrandProduct { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public ProductType ProductType { get; set; }
        public Status Status { get; set; }
        public TaxType TaxType { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public UnitOfMeasureGrouping UnitOfMeasureGrouping { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ProductFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter SupplierCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public StringFilter ScanCode { get; set; }
        public StringFilter ERPCode { get; set; }
        public IdFilter CategoryId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public IdFilter CodeGeneratorRuleId { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter TaxTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter TechnicalName { get; set; }
        public StringFilter Note { get; set; }
        public bool? IsPurchasable { get; set; }
        public bool? IsSellable { get; set; }
        public bool? IsNew { get; set; }
        public IdFilter UsedVariationId { get; set; }
        public bool? Used { get; set; }
        public bool? IsCopyRightedProduct { get; set; }
        public bool? IsBrandProduct { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ProductFilter> OrFilter { get; set; }
        public ProductOrder OrderBy {get; set;}
        public ProductSelect Selects {get; set;}
        public ProductSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductOrder
    {
        Id = 0,
        Code = 1,
        SupplierCode = 2,
        Name = 3,
        Description = 4,
        ScanCode = 5,
        ERPCode = 6,
        Category = 7,
        ProductType = 8,
        Brand = 9,
        UnitOfMeasure = 10,
        CodeGeneratorRule = 11,
        UnitOfMeasureGrouping = 12,
        SalePrice = 13,
        RetailPrice = 14,
        TaxType = 15,
        Status = 16,
        OtherName = 17,
        TechnicalName = 18,
        Note = 19,
        IsPurchasable = 20,
        IsSellable = 21,
        IsNew = 22,
        UsedVariation = 23,
        Used = 27,
        IsCopyRightedProduct = 29,
        IsBrandProduct = 30,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ProductSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        SupplierCode = E._2,
        Name = E._3,
        Description = E._4,
        ScanCode = E._5,
        ERPCode = E._6,
        Category = E._7,
        ProductType = E._8,
        Brand = E._9,
        UnitOfMeasure = E._10,
        CodeGeneratorRule = E._11,
        UnitOfMeasureGrouping = E._12,
        SalePrice = E._13,
        RetailPrice = E._14,
        TaxType = E._15,
        Status = E._16,
        OtherName = E._17,
        TechnicalName = E._18,
        Note = E._19,
        IsPurchasable = E._20,
        IsSellable = E._21,
        IsNew = E._22,
        UsedVariation = E._23,
        Used = E._27,
        IsCopyRightedProduct = E._29,
        IsBrandProduct = E._30,
    }

    [Flags]
    public enum ProductSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        SupplierCode = E._2,
        Name = E._3,
        Description = E._4,
        ScanCode = E._5,
        ERPCode = E._6,
        OtherName = E._17,
        TechnicalName = E._18,
        Note = E._19,
    }
}
