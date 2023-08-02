namespace IWM.Services.MBrand
{
    public class BrandMessage
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
        }
    }
}
