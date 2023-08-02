namespace IWM.Services.MDistrict
{
    public class DistrictMessage
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
            ProvinceEmpty,
            ProvinceNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
