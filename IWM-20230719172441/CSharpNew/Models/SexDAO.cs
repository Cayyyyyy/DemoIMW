using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class SexDAO
    {
        public SexDAO()
        {
            AppUsers = new HashSet<AppUserDAO>();
            Workers = new HashSet<WorkerDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AppUserDAO> AppUsers { get; set; }
        public virtual ICollection<WorkerDAO> Workers { get; set; }
    }
}
