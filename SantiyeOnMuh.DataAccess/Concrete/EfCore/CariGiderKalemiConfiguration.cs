using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    internal class CariGiderKalemiConfiguration : IEntityTypeConfiguration<ECariGiderKalemi>
    {
        public void Configure(EntityTypeBuilder<ECariGiderKalemi> builder)
        {
            builder.HasData(
                new ECariGiderKalemi() {Id=1, Ad = "ÇEK", Durum = true, Tur = false },
                new ECariGiderKalemi() {Id=2, Ad = "NAKİT", Durum = true, Tur = false }
                );
        }
    }
}
