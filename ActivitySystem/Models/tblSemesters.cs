using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblSemesters
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public string SemesterName { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDone { get; set; }
    }
}
