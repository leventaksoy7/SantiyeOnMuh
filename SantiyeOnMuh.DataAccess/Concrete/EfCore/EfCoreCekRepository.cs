using Microsoft.EntityFrameworkCore;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCekRepository : EfCoreGenericRepository<ECek>, ICekRepository
    {
        public EfCoreCekRepository(Context context): base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ECek> GetAll(bool drm)
        {

            return Context.Ceks
                .Where(i => i.Durum == drm)
                .ToList();

        }
        public ECek GetByIdDetay(int id)
        {

            return Context.Ceks.
                    Include(y => y.Sirket)
                .Include(x => x.CariHesap)
                .Include(x => x.BankaHesap)
                .FirstOrDefault(x => x.Id == id);

        }
        public List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {

                var cek = Context.Ceks.AsQueryable();

                if (santiyeid != null)
                {
                    cek = cek
                        .Where(i => i.Durum == drm)
                        .Where(i => i.CariHesap.SantiyeId == santiyeid);
                }
                else if (sirketid != null)
                {
                    cek = cek
                        .Where(i => i.Durum == drm)
                        .Where(i => i.SirketId == sirketid);
                }
                else if (bankahesapid != null)
                {
                    cek = cek
                        .Where(i => i.Durum == drm)
                        .Where(i => i.BankaHesapId == bankahesapid);
                }
                else
                {
                    cek = cek
                        .Where(i => i.Durum == drm);
                }
                return cek
                    .OrderBy(i => i.Tarih)
                    .Include(i => i.CariHesap)
                    .Include(i => i.Sirket)
                    .Include(i => i.BankaHesap)
                    .ToList();

        }
        public List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {

            var cek = Context.Ceks.AsQueryable();

            if (santiyeid != null)
            {
                cek = cek
                    .Where(i => i.Durum == drm)
                    .Where(i => i.CariHesap.SantiyeId == santiyeid);
            }
            else if (sirketid != null)
            {
                cek = cek
                    .Where(i => i.Durum == drm)
                    .Where(i => i.SirketId == sirketid);
            }
            else if (bankahesapid != null)
            {
                cek = cek
                    .Where(i => i.Durum == drm)
                    .Where(i => i.BankaHesapId == bankahesapid);
            }
            else
            {
                cek = cek
                    .Where(i => i.Durum == drm);
            }
            return cek
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

            var cek = Context.Ceks.AsQueryable();

            if (santiyeid != null)
            {
                cek = cek.Where(i => i.Durum == drm).Where(i => i.CariHesap.SantiyeId == santiyeid);
            }
            else if (sirketid != null)
            {
                cek = cek.Where(i => i.Durum == drm).Where(i => i.SirketId == sirketid);
            }
            else if (bankahesapid != null)
            {
                cek = cek.Where(i => i.Durum == drm).Where(i => i.BankaHesapId == bankahesapid);
            }
            else
            {
                cek = cek
                    .Where(i => i.Durum == drm);
            }
            return cek
                .Include(i => i.CariHesap)
                .ThenInclude(i => i.Santiye)
                .Include(i => i.Sirket)
                .Include(i => i.BankaHesap)
                .Count();

        }
    }
}
