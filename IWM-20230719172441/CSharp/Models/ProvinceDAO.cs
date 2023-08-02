using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class ProvinceDAO
    {
        public ProvinceDAO()
        {
            Districts = new HashSet<DistrictDAO>();
            ProvinceProvinceGroupingMappings = new HashSet<ProvinceProvinceGroupingMappingDAO>();
            Workers = new HashSet<WorkerDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<DistrictDAO> Districts { get; set; }
        public virtual ICollection<ProvinceProvinceGroupingMappingDAO> ProvinceProvinceGroupingMappings { get; set; }
        public virtual ICollection<WorkerDAO> Workers { get; set; }
    }
}
