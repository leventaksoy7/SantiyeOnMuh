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
    public class EfCoreCariKasaRepository : EfCoreGenericRepository<ECariKasa>, ICariKasaRepository
    {
        public EfCoreCariKasaRepository(Context context):base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ECariKasa> GetAll(bool drm)
        {

            return Context.CariKasas
                .Where(i => i.Durum == drm)
                .ToList();

        }

        public ECariKasa GetByIdDetay(int id)
        {

            return Context.CariKasas
                .Include(i => i.CariHesap)
                .ThenInclude(i => i.Santiye)
                .Include(x => x.CariGiderKalemi)
                .FirstOrDefault(x => x.Id == id);
  
        }
        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm)
        {

            var carikasa = Context.CariKasas.AsQueryable();

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

        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize)
        {

            var carikasa = Context.CariKasas.AsQueryable();

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
        public int GetCount(int carihesapid, int? gkid, bool drm)
        {

            var carikasa = Context.CariKasas.AsQueryable();

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
