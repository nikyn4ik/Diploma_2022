using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPassword.Properties
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
      : base("SPUTNIK") { }
        public DbSet <authorization> authorization { get; set; }
    }
}
