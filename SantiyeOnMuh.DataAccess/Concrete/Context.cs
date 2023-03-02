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
        public DbSet<ESantiye> Santiyes { get; set; }
        public DbSet<ESantiyeKasa> SantiyesKasas { get; set; }
        public DbSet<ESantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
        public DbSet<ECariHesap> CariHesaps { get; set; }
        public DbSet<ECariKasa> CariKasas { get; set; }
        public DbSet<ECariGiderKalemi> CariGiderKalemis { get; set; }
        public DbSet<EBankaHesap> BankaHesaps { get; set; }
        public DbSet<EBankaKasa> BankaKasas { get; set; }
        public DbSet<ECek> Ceks { get; set; }
        public DbSet<ENakit> Nakits { get; set; }
        public DbSet<ESirket> Sirkets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS; Database=SantiyeOnMuhasebe; Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");
        }

    }
}
