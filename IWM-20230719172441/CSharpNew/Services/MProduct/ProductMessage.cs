namespace IWM.Services.MProduct
{
    public class ProductMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            IdNotExisted,
            ObjectUsed,
            CodeHasSpecialCharacter,
            CodeExisted,
            CodeEmpty,
            CodeOverLength,
            SupplierCodeEmpty,
            SupplierCodeOverLength,
            NameEmpty,
            NameOverLength,
            DescriptionEmpty,
            DescriptionOverLength,
            ScanCodeEmpty,
            ScanCodeOverLength,
            ERPCodeEmpty,
            ERPCodeOverLength,
            SalePriceInvalid,
            RetailPriceInvalid,
            OtherNameEmpty,
            OtherNameOverLength,
            TechnicalNameEmpty,
            TechnicalNameOverLength,
            NoteEmpty,
            NoteOverLength,
            BrandNotExisted,
            CategoryEmpty,
            CategoryNotExisted,
            ProductTypeEmpty,
            ProductTypeNotExisted,
            StatusEmpty,
            StatusNotExisted,
            TaxTypeEmpty,
            TaxTypeNotExisted,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
            UnitOfMeasureGroupingNotExisted,
        }
    }
}
