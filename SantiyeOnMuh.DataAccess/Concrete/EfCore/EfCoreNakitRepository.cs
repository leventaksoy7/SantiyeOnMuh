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
    public class EfCoreNakitRepository : EfCoreGenericRepository<Nakit, Context>, INakitRepository
    {
        public List<Nakit> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.Nakits
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }
        public Nakit GetByIdDetay(int id)
        {
            using (var context = new Context())
            {
                return context.Nakits.
                     Include(y => y.Sirket)
                    .Include(x => x.CariHesap)
                    .Include(x => x.BankaHesap)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        public List<Nakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var nakit = context.Nakits.AsQueryable();

                if (santiyeid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.CariHesap.SantiyeId == santiyeid);
                }
                else if (sirketid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.SirketId == sirketid);
                }
                else if (bankahesapid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.BankaHesapId == bankahesapid);
                }
                else
                {
                    nakit = nakit
                        .Where(i => i.Durum == drm);
                }
                return nakit
                    .OrderBy(s => s.Tarih)
                    .Include(i => i.CariHesap)
                    .ThenInclude(i => i.Santiye)
                    .Include(i => i.Sirket)
                    .Include(i => i.BankaHesap)
                    .ToList();
            }
        }
        public List<Nakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            using (var context = new Context())
            {
                var nakit = context.Nakits.AsQueryable();

                if (santiyeid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.CariHesap.SantiyeId == santiyeid);
                }
                else if (sirketid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.SirketId == sirketid);
                }
                else if (bankahesapid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.BankaHesapId == bankahesapid);
                }
                else
                {
                    nakit = nakit
                        .Where(i => i.Durum == drm);
                }
                return nakit
                    .OrderBy(s => s.Tarih)
                    .Reverse()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(t => t.Tarih)
                    .Include(i => i.CariHesap)
                    .ThenInclude(i => i.Santiye)
                    .Include(i => i.Sirket)
                    .Include(i => i.BankaHesap)
                    .ToList();
            }
        }
        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var nakit = context.Nakits.AsQueryable();

                if (santiyeid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.CariHesap.SantiyeId == santiyeid);
                }
                else if (sirketid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.SirketId == sirketid);
                }
                else if (bankahesapid != null)
                {
                    nakit = nakit.Where(i => i.Durum == drm).Where(i => i.BankaHesapId == bankahesapid);
                }
                else
                {
                    nakit = nakit
                        .Where(i => i.Durum == drm);
                }
                return nakit
                    .Include(i => i.CariHesap)
                    .ThenInclude(i => i.Santiye)
                    .Include(i => i.Sirket)
                    .Include(i => i.BankaHesap)
                    .Count();
            }
        }
    }
}
