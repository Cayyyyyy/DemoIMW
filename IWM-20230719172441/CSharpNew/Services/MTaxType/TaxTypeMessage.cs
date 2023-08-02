namespace IWM.Services.MTaxType
{
    public class TaxTypeMessage
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
            PercentageInvalid,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
