using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class CurrentUser : EnumEntity
    {
        public static CurrentUser ISNT = new CurrentUser (Id : 0, Code : "ISNT", Name : "Không phải tài khoản hiện tại");
        public static CurrentUser IS = new CurrentUser (Id : 1, Code : "IS", Name : "Là tài khoản hiện tại");
        public static List<CurrentUser> CurrentUserEnumList = new List<CurrentUser>
        {
            IS,ISNT,
        };
        public CurrentUser() : base(nameof(CurrentUser)) { }
        public CurrentUser(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(CurrentUser), Color, Value) { }
    }
}
