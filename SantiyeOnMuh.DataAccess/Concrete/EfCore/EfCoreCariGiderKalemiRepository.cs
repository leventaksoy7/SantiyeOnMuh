using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCariGiderKalemiRepository : EfCoreGenericRepository<ECariGiderKalemi, Context>, ICariGiderKalemiRepository
    {
        public List<ECariGiderKalemi> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.CariGiderKalemis
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }

        public List<ECariGiderKalemi> GetAll(bool drm, bool tur)
        {
            using (var context = new Context())
            {
                return context.CariGiderKalemis
                    .Where(i => i.Tur == drm)
                    .Where(i => i.Durum == drm)
                    .OrderBy(i => i.Ad)
                    .ToList();
            }
        }
    }
}
