namespace IWM.Services.MUnitOfMeasureGrouping
{
    public class UnitOfMeasureGroupingMessage
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
            StatusEmpty,
            StatusNotExisted,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
        }
    }
}
