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
    public class SantiyeGiderKalemiConfiguration : IEntityTypeConfiguration<ESantiyeGiderKalemi>
    {
        public void Configure(EntityTypeBuilder<ESantiyeGiderKalemi> builder)
        {
            builder.HasData(
                new ESantiyeGiderKalemi() {Id=1, Ad = "ŞANTİYE KASASINA EFT", Durum = true, Tur = false }
                );
        }
    }
}
