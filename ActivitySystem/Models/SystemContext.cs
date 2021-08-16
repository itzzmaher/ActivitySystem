using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ActivitySystem.Models
{
    public class SystemContext : DbContext
    {
        public DbSet<tblUsers> tblUsers { get; set; }
        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblColleges> tblColleges { get; set; }
        public DbSet<tblActivities> tblActivities { get; set; }
        public DbSet<tblStatus> tblStatus { get; set; }
        public DbSet<tblActivityLogs> tblActivityLogs { get; set; }
        public DbSet<tblRegisterations> tblRegisterations { get; set; }
        public DbSet<tblRegisterationsLogs> tblRegisterationsLogs { get; set; }
        public DbSet<tblSemesters> tblSemesters { get; set; }


        public SystemContext(DbContextOptions<SystemContext> options)
            : base(options)
        {

        }
        public SystemContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBCS"));
        }
    }
}
