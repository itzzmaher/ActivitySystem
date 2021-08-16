using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblActivities
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public string ActivityName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RegisterStartDate { get; set; }
        public DateTime? RegisterEndDate { get; set; }
        public int MaxStudents { get; set; }
        public tblUsers Creator { get; set; }
        public int CreatorId { get; set; } 
        public tblUsers Supervisor { get; set; }
        public int SupervisorId { get; set; }
        public tblSemesters Semester { get; set; }
        public int SemesterId { get; set; }
        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
