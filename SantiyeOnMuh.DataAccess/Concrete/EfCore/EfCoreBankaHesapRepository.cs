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
    public class EfCoreBankaHesapRepository : EfCoreGenericRepository<EBankaHesap>, IBankaHesapRepository
    {
        public EfCoreBankaHesapRepository(Context context): base(context)
        {
            
        }

        private Context Context 
        { 
            get { return context as Context; } 
        }

        public List<EBankaHesap> GetAll(bool drm)
        {
            var BankaHesaps = Context.BankaHesaps.AsQueryable();

            return BankaHesaps
                .Where(i => i.Durum == drm)
                .ToList();

        }
    }
}
