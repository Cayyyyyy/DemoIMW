using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class WorkerGroupDAO
    {
        public WorkerGroupDAO()
        {
            Workers = new HashSet<WorkerDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<WorkerDAO> Workers { get; set; }
    }
}
