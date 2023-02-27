using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreBankaKasaRepository : EfCoreGenericRepository<BankaKasa, Context>, IBankaKasaRepository
    {
        public List<BankaKasa> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.BankaKasas
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
