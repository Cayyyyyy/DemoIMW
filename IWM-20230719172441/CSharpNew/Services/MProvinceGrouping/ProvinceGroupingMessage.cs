namespace IWM.Services.MProvinceGrouping
{
    public class ProvinceGroupingMessage
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
            LevelInvalid,
            PathEmpty,
            PathOverLength,
            ParentNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
