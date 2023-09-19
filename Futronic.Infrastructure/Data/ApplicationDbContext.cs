using Futronic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futronic.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Data Source=172.10.10.35/MSSQLSERVER;Initial Catalog = DatabaseEnrollement; User ID = sa; Password=Admin@@2020")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<FingerPrint> FingerPrints { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
    }
}
