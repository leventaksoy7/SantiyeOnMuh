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
    public class EfCoreCariKasaRepository : EfCoreGenericRepository<CariKasa, Context>, ICariKasaRepository
    {
        public List<CariKasa> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.CariKasas
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }

        public CariKasa GetByIdDetay(int id)
        {
            using (var context = new Context())
            {
                return context.CariKasas
                    .Include(i => i.CariHesap)
                    .ThenInclude(i => i.Santiye)
                    .Include(x => x.CariGiderKalemi)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        public List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm)
        {
            using (var context = new Context())
            {
                var carikasa = context.CariKasas.AsQueryable();

                if (gkid == null)
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid)
                            .Include(x => x.CariGiderKalemi);
                }
                else
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(x => x.CariGiderKalemi)
                            .Where(i => i.CariGiderKalemiId == gkid)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid);
                }

                return carikasa
                    .OrderBy(s => s.Tarih)
                    .Reverse()
                    .OrderBy(s => s.Tarih)
                    .ToList();
            }
        }

        public List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize)
        {
            using (var context = new Context())
            {
                var carikasa = context.CariKasas.AsQueryable();

                if (gkid == null)
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid)
                            .Include(x => x.CariGiderKalemi);
                }
                else
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(x => x.CariGiderKalemi)
                            .Where(i => i.CariGiderKalemiId == gkid)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid);
                }

                return carikasa
                    .OrderBy(s => s.Tarih)
                    .Reverse()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(s => s.Tarih)
                    .ToList();
            }
        }
        public int GetCount(int carihesapid, int? gkid, bool drm)
        {
            using (var context = new Context())
            {
                var carikasa = context.CariKasas.AsQueryable();

                if (gkid == null)
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid)
                            .Include(x => x.CariGiderKalemi);
                }
                else
                {
                    carikasa = carikasa
                            .Where(i => i.Durum == drm)
                            .Include(x => x.CariGiderKalemi)
                            .Where(i => i.CariGiderKalemiId == gkid)
                            .Include(i => i.CariHesap)
                            .Where(i => i.CariHesapId == carihesapid);
                }

                return carikasa.Count();
            }
        }
    }
}
