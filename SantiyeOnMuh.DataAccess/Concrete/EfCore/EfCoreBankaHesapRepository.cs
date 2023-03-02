using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreBankaHesapRepository : EfCoreGenericRepository<BankaHesap, Context>, IBankaHesapRepository
    {
        public List<BankaHesap> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                var BankaHesaps = context.BankaHesaps.AsQueryable();

                return BankaHesaps
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }
    }
}
