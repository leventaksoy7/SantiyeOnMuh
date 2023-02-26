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
        public DbSet<SantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
        public DbSet<CariHesap> CariHesaps { get; set; }
        public DbSet<CariKasa> CariKasas { get; set; }
        public DbSet<CariGiderKalemi> CariGiderKalemis { get; set; }
        public DbSet<BankaHesap> BankaHesaps { get; set; }
        public DbSet<BankaKasa> BankaKasas { get; set; }
        public DbSet<OdemeCek> OdemeCeks { get; set; }
        public DbSet<OdemeNakit> OdemeNakits { get; set; }
        public DbSet<Sirket> Sirkets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS; Database=SantiyeOnMuhasebe; Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");
        }

    }
}
