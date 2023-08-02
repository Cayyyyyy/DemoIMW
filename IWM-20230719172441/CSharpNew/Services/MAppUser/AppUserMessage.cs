namespace IWM.Services.MAppUser
{
    public class AppUserMessage
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
            UsernameEmpty,
            UsernameOverLength,
            DisplayNameEmpty,
            DisplayNameOverLength,
            AddressEmpty,
            AddressOverLength,
            EmailEmpty,
            EmailOverLength,
            PhoneEmpty,
            PhoneOverLength,
            BirthdayInvalid,
            BirthdayEmpty,
            AvatarEmpty,
            AvatarOverLength,
            DepartmentEmpty,
            DepartmentOverLength,
            CodeEmpty,
            CodeOverLength,
            NameEmpty,
            NameOverLength,
            OrganizationEmpty,
            OrganizationNotExisted,
            SexEmpty,
            SexNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
