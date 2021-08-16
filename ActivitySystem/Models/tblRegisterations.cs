using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblRegisterations
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public DateTime RegisterationTime { get; set; }
        public tblUsers User { get; set; }
        public int UserId { get; set; }
        public tblActivities Activity { get; set; }
        public int ActivityId { get; set; }
        public tblStatus StatusReg { get; set; }
        public int StatusRegId { get; set; }


    }
}
