using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class Sex : EnumEntity
    {
        public static Sex MALE => new Sex(Id: 1, Name: "Nam", Code: "Male");
        public static Sex FEMALE => new Sex(Id: 2, Name: "Nữ", Code: "Female");
        public static Sex OTHER => new Sex(Id: 3, Name: "Khác", Code: "Other");
        public static List<Sex> SexEnumList = new List<Sex>
        {
            MALE, FEMALE, OTHER,
        };
        public Sex() : base(nameof(Sex)) { }
        public Sex(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(Sex), Color, Value) { }
    }

    public class SexFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<SexFilter> OrFilter { get; set; }
        public SexOrder OrderBy { get; set; }
        public SexSelect Selects { get; set; } = SexSelect.ALL;
        public SexSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SexOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum SexSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }

    [Flags]
    public enum SexSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
    }
}
