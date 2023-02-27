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
    public class EfCoreCariHesapRepository : EfCoreGenericRepository<CariHesap, Context>, ICariHesapRepository
    {
        public List<CariHesap> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.CariHesaps
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }

        public List<CariHesap> GetAll(int? santiyeid, bool drm)
        {
            using (var context = new Context())
            {
                var carihesap = context.CariHesaps.AsQueryable();

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
        }

        public List<CariHesap> GetAll(int? santiyeid, bool drm, int page, int pageSize)
        {
            using (var context = new Context())
            {
                var carihesap = context.CariHesaps.AsQueryable();

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
        }
        public int GetCountBySantiye(int? santiyeid, bool drm)
        {
            using (var context = new Context())
            {
                var carihesap = context.CariHesaps.AsQueryable();

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
}
