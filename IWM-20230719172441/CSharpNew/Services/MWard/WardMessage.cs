namespace IWM.Services.MWard
{
    public class WardMessage
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
            DistrictEmpty,
            DistrictNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
