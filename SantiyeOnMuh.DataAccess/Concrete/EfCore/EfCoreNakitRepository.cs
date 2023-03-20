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
    public class EfCoreNakitRepository : EfCoreGenericRepository<ENakit>, INakitRepository
    {
        public EfCoreNakitRepository(Context context): base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ENakit> GetAll(bool drm)
        {

                return Context.Nakits
                    .Where(i => i.Durum == drm)
                    .ToList();

        }
        public ENakit GetByIdDetay(int id)
        {

                return Context.Nakits.
                     Include(y => y.Sirket)
                    .Include(x => x.CariHesap)
                    .Include(x => x.BankaHesap)
                    .FirstOrDefault(x => x.Id == id);

        }
        public List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {

                var nakit = Context.Nakits.AsQueryable();

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
        public List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {

                var nakit = Context.Nakits.AsQueryable();

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
        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {

                var nakit = Context.Nakits.AsQueryable();

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
