using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class WorkerDAO
    {
        public WorkerDAO()
        {
            WorkerDistrictWorkingMappings = new HashSet<WorkerDistrictWorkingMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentificationNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? SexId { get; set; }
        public long? WorkerGroupId { get; set; }
        public long? NationId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual DistrictDAO District { get; set; }
        public virtual NationDAO Nation { get; set; }
        public virtual ProvinceDAO Province { get; set; }
        public virtual SexDAO Sex { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual WardDAO Ward { get; set; }
        public virtual WorkerGroupDAO WorkerGroup { get; set; }
        public virtual ICollection<WorkerDistrictWorkingMappingDAO> WorkerDistrictWorkingMappings { get; set; }
    }
}
