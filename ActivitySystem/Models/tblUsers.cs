using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivitySystem.Models
{
    public class tblUsers
    {

        public int Id { get; set; }
        public Guid GuId { get; set; }
        public string Name { get; set; }
        public string KfuEmail { get; set; }
        public string Password { get; set; }
        public tblColleges College { get; set; }
        public int? CollegeId { get; set; }
        public tblRoles Role { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsGraduate { get; set; }

    }
}
