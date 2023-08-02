using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWM.Services.MOrganization
{
    public class OrganizationMessage
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
            PathEmpty,
            PathOverLength,
            LevelInvalid,
            PhoneEmpty,
            PhoneOverLength,
            EmailEmpty,
            EmailOverLength,
            AddressEmpty,
            AddressOverLength,
            TaxCodeEmpty,
            TaxCodeOverLength,
            ParentNotExisted,
            StatusEmpty,
            StatusNotExisted,
        }
    }
}
