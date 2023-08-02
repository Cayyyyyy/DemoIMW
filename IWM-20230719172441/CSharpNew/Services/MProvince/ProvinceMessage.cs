namespace IWM.Services.MProvince
{
    public class ProvinceMessage
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
            PriorityInvalid,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
