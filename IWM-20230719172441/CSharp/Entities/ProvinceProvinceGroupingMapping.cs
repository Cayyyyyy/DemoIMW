using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class ProvinceProvinceGroupingMapping : DataEntity
    {
        public long ProvinceGroupingId { get; set; }
        public long ProvinceId { get; set; }
        public Province Province { get; set; }
        public ProvinceGrouping ProvinceGrouping { get; set; }
    }

    public class ProvinceProvinceGroupingMappingFilter : FilterEntity
    {
        public IdFilter ProvinceGroupingId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public List<ProvinceProvinceGroupingMappingFilter> OrFilter { get; set; }
        public ProvinceProvinceGroupingMappingOrder OrderBy {get; set;}
        public ProvinceProvinceGroupingMappingSelect Selects {get; set;}
        public ProvinceProvinceGroupingMappingSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProvinceProvinceGroupingMappingOrder
    {
        ProvinceGrouping = 0,
        Province = 1,
    }

    [Flags]
    public enum ProvinceProvinceGroupingMappingSelect:long
    {
        ALL = E.ALL,
        ProvinceGrouping = E._0,
        Province = E._1,
    }

    [Flags]
    public enum ProvinceProvinceGroupingMappingSearch:long
    {
        ALL = E.ALL,
    }
}
