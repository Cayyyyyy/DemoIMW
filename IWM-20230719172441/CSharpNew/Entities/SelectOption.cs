using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class SelectOption : EnumEntity
    {
        public static SelectOption SELECTED = new SelectOption(1, "SELECTED", "Đã chọn");
        public static SelectOption NOT_SELECTED = new SelectOption(2, "NOT_SELECTED", "Chưa chọn");
        public static SelectOption ALL = new SelectOption(3, "ALL", "Tất cả");
        public static List<SelectOption> SelectOptionEnumList = new List<SelectOption>
        {
            SELECTED, NOT_SELECTED, ALL,
        };
        public SelectOption() : base(nameof(SelectOption)) { }
        public SelectOption(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(SelectOption), Color, Value)
        { }
    }
}
