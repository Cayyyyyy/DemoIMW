namespace IWM.Services.MCategory
{
    public class CategoryMessage
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
            PrefixEmpty,
            PrefixOverLength,
            DescriptionEmpty,
            DescriptionOverLength,
            PathEmpty,
            PathOverLength,
            LevelInvalid,
            OrderNumberInvalid,
            ImageNotExisted,
            ParentNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
