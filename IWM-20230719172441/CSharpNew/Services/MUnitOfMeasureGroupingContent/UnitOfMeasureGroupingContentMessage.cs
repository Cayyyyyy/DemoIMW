namespace IWM.Services.MUnitOfMeasureGroupingContent
{
    public class UnitOfMeasureGroupingContentMessage
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
            FactorInvalid,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
            UnitOfMeasureGroupingEmpty,
            UnitOfMeasureGroupingNotExisted,
        }
    }
}
