namespace IWM.Services.MNation
{
    public class NationMessage
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
