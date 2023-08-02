using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class WorkerDistrictWorkingMappingDAO
    {
        public long WorkerId { get; set; }
        public long DistrictId { get; set; }

        public virtual DistrictDAO District { get; set; }
        public virtual WorkerDAO Worker { get; set; }
    }
}
