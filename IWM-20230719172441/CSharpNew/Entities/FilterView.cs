using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class SystemFilterView : EnumEntity
    {
        public static SystemFilterView ALL = new SystemFilterView(Id: 1, Code: "ALL", Name: "Tất cả", Value: "{}");
        public static SystemFilterView MINE = new SystemFilterView(Id: 2, Code: "MINE", Name: "Của tôi", Value: "{ \"IsOwned\":\"true\" }");
        public static SystemFilterView SUBMITTOME = new SystemFilterView(Id: 3, Code: "SUBMITTOME", Name: "Tôi duyệt", Value: "{ \"IsPending\":\"true\" }");

        public static List<SystemFilterView> SystemFilterViewList = new List<SystemFilterView>()
        {
            ALL, MINE, SUBMITTOME,
        };
        public SystemFilterView() : base(nameof(SystemFilterView)) { }
        public SystemFilterView(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(SystemFilterView), Color, Value)
        { }
    }
    public class FilterViewType : EnumEntity
    {
        public static FilterViewType SYSTEM = new FilterViewType(Id: 1, Code: "SYSTEM", Name: "Tự sinh");
        public static FilterViewType CUSTOMIZE = new FilterViewType(Id: 2, Code: "CUSTOMIZE", Name: "Tùy chỉnh");
        public static FilterViewType SHARED = new FilterViewType(Id: 3, Code: "SHARED", Name: "Được chia sẻ");
        public static List<FilterViewType> FilterViewTypeEnumList = new List<FilterViewType>
        {
            SYSTEM, CUSTOMIZE, SHARED
        };
        public FilterViewType() : base(nameof(FilterViewType)) { }
        public FilterViewType(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(FilterViewType), Color, Value)
        { }
    }
}
