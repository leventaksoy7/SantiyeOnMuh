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
    public class EfCoreSantiyeKasaRepository : EfCoreGenericRepository<ESantiyeKasa>, ISantiyeKasaRepository
    {
        public EfCoreSantiyeKasaRepository(Context context):base (context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ESantiyeKasa> GetAll(int? santiyeid, int? gkid, bool drm)
        {

            var santiyekasa = Context.SantiyesKasas.AsQueryable();

            if (santiyeid != null)
            {
                if (gkid == null)
                {
                    santiyekasa = santiyekasa
                        .Include(i => i.Santiye)
                        .Include(i => i.SantiyeGiderKalemi)
                        .Where(i => i.Durum == drm)
                        .Where(i => i.SantiyeId == santiyeid);

                }
                else
                {
                    santiyekasa = santiyekasa
                        .Include(i => i.Santiye)
                        .Include(i => i.SantiyeGiderKalemi)
                        .Where(i => i.Durum == drm)
                        .Where(i => i.SantiyeGiderKalemiId == gkid)
                        .Where(i => i.SantiyeId == santiyeid);
                }
            }
            else
            {
                santiyekasa = santiyekasa
                    .Include(i => i.Santiye)
                    .Include(i => i.SantiyeGiderKalemi)
                    .Where(i => i.Durum == drm);
            }

            return santiyekasa
                .OrderByDescending(i => i.Tarih)
                .ToList();

        }

        public List<ESantiyeKasa> GetAll(int santiyeid, int? gkid, bool drm, int page, int pageSize)
        {

            var santiyekasa = Context.SantiyesKasas.AsQueryable();

            if (gkid == null)
            {
                santiyekasa = santiyekasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid)
                        .Include(i => i.SantiyeGiderKalemi);
            }
            else
            {
                santiyekasa = santiyekasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid)
                        .Include(i => i.SantiyeGiderKalemi)
                        .Where(i => i.SantiyeGiderKalemiId == gkid);
            }
            return santiyekasa
                .OrderBy(i => i.Tarih)
                .Reverse()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(j => j.Tarih)
                .ToList();

        }

        public ESantiyeKasa GetByIdDetay(int id)
        {

            return Context.SantiyesKasas
                .Include(i => i.Santiye)
                .Include(i => i.SantiyeGiderKalemi)
                .FirstOrDefault(i => i.Id == id);

        }

        public int GetCount(int santiyeid, int? gkid, bool drm)
        {

            var santiyekasa = Context.SantiyesKasas.AsQueryable();

            if (gkid == null)
            {
                santiyekasa = santiyekasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid)
                        .Include(i => i.SantiyeGiderKalemi);
            }
            else
            {
                santiyekasa = santiyekasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.Santiye)
                        .Where(i => i.SantiyeId == santiyeid)
                        .Include(i => i.SantiyeGiderKalemi)
                        .Where(i => i.SantiyeGiderKalemiId == gkid);
            }
            return santiyekasa.Count();

        }
    }
}
