using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblRegisterationsLogs
    {
        public int Id { get; set; }
        public tblUsers User { get; set; }
        public int UserId { get; set; }
        public tblRegisterations Registeration { get; set; }
        public int RegisterationId { get; set; }
        public DateTime UpdateDate { get; set; }
        public tblStatus Status { get; set; }
        public int StatusId { get; set; }
    }
}
