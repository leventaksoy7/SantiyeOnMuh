using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSantiyeGiderKalemiRepository : EfCoreGenericRepository<SantiyeGiderKalemi, Context>, ISantiyeGiderKalemiRepository
    {
        public List<SantiyeGiderKalemi> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.SantiyeGiderKalemis
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }
        public List<SantiyeGiderKalemi> GetAll(bool drm, bool tur)
        {
            using (var context = new Context())
            {
                return context.SantiyeGiderKalemis
                    .Where(i => i.Tur == drm)
                    .Where(i => i.Durum == drm)
                    .OrderBy(i => i.Ad)
                    .ToList();
            }
        }
    }
}
