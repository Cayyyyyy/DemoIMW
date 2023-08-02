using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class ProvinceProvinceGroupingMappingDAO
    {
        public long ProvinceGroupingId { get; set; }
        public long ProvinceId { get; set; }

        public virtual ProvinceDAO Province { get; set; }
        public virtual ProvinceGroupingDAO ProvinceGrouping { get; set; }
    }
}
