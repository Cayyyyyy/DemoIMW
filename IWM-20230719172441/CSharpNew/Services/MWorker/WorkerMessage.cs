namespace IWM.Services.MWorker
{
    public class WorkerMessage
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
            BirthdayInvalid,
            BirthdayEmpty,
            PhoneEmpty,
            PhoneOverLength,
            CitizenIdentificationNumberEmpty,
            CitizenIdentificationNumberOverLength,
            EmailEmpty,
            EmailOverLength,
            AddressEmpty,
            AddressOverLength,
            UsernameEmpty,
            UsernameOverLength,
            PasswordEmpty,
            PasswordOverLength,
            DistrictNotExisted,
            NationNotExisted,
            ProvinceNotExisted,
            SexNotExisted,
            StatusEmpty,
            StatusNotExisted,
            WardNotExisted,
            WorkerGroupNotExisted,
        }
    }
}
