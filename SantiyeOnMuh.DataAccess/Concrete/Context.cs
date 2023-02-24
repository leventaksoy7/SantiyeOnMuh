using Microsoft.EntityFrameworkCore;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete
{
    public class Context:DbContext
    {
        public DbSet<Santiye> Santiyes { get; set; }
        public DbSet<SantiyeKasa> SantiyesKasa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\SQLEXPRESS;database=SantiyeOnMuh; integrated security=true");
        }

    }
}
