using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblActivityLogs
    {
        public int Id { get; set; }
        public tblUsers User { get; set; }
        public int UserId { get; set; }
        public tblActivities Activity { get; set; }
        public int ActivityId { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
