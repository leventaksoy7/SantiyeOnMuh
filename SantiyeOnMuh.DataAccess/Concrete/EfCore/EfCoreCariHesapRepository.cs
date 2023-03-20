using Microsoft.EntityFrameworkCore;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCariHesapRepository : EfCoreGenericRepository<ECariHesap>, ICariHesapRepository
    {
        public EfCoreCariHesapRepository(Context context ):base (context)
        {
            
        }

        private Context Context
        {
            get { return context as Context; }
        }

        public List<ECariHesap> GetAll(bool drm)
        {

            return Context.CariHesaps
                .Where(i => i.Durum == drm)
                .ToList();

        }

        public List<ECariHesap> GetAll(int? santiyeid, bool drm)
        {

            var carihesap = Context.CariHesaps.AsQueryable();

            if (santiyeid == null)
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye);
            }
            else
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid);
            }

            return carihesap
                .OrderBy(i => i.Ad)
                .ToList();

        }

        public List<ECariHesap> GetAll(int? santiyeid, bool drm, int page, int pageSize)
        {

            var carihesap = Context.CariHesaps.AsQueryable();

            if (santiyeid == null)
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye);
            }
            else
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid);
            }

            return carihesap
                .OrderBy(f => f.Ad)
                .Reverse()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(f => f.Ad)
                .ToList();

        }
        public int GetCount(int? santiyeid, bool drm)
        {

            var carihesap = Context.CariHesaps.AsQueryable();

            if (santiyeid == null)
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye);
            }
            else
            {
                carihesap = carihesap
                        .Where(c => c.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid);
            }

            return carihesap.Count();

        }
    }
}
