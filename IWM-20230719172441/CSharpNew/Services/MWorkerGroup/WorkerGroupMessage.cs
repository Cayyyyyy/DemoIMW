namespace IWM.Services.MWorkerGroup
{
    public class WorkerGroupMessage
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
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
