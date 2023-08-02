using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class ProvinceGroupingDAO
    {
        public ProvinceGroupingDAO()
        {
            InverseParent = new HashSet<ProvinceGroupingDAO>();
            ProvinceProvinceGroupingMappings = new HashSet<ProvinceProvinceGroupingMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public long? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public long Level { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ProvinceGroupingDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<ProvinceGroupingDAO> InverseParent { get; set; }
        public virtual ICollection<ProvinceProvinceGroupingMappingDAO> ProvinceProvinceGroupingMappings { get; set; }
    }
}
