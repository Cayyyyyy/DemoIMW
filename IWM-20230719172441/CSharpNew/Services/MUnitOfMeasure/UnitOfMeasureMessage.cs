namespace IWM.Services.MUnitOfMeasure
{
    public class UnitOfMeasureMessage
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
            NameEmpty,
            NameOverLength,
            DescriptionEmpty,
            DescriptionOverLength,
            ErpCodeEmpty,
            ErpCodeOverLength,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
