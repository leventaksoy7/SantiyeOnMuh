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
    public class EfCoreBankaKasaRepository : EfCoreGenericRepository<EBankaKasa>, IBankaKasaRepository
    {
        public EfCoreBankaKasaRepository(Context context) : base(context)
        {
            
        }

        private Context Context
        {
            get { return context as Context;}
        }

        public EBankaKasa GetByIdDetay(int id)
        {
            return Context.BankaKasas
                .Include(i => i.BankaHesap)
                .FirstOrDefault(i => i.Id == id);

        }

        public List<EBankaKasa> GetAll(bool drm)
        {

            return Context.BankaKasas
                .Where(i => i.Durum == drm)
                .ToList();

        }

        public List<EBankaKasa> GetAll(int? bankahesapid, bool drm)
        {

            var bankakasa = Context.BankaKasas.AsQueryable();

            if (bankahesapid == null)
            {
                bankakasa = bankakasa
                    .Include(i => i.BankaHesap)
                    .Where(i => i.Durum == drm);
            }
            else
            {
                bankakasa = bankakasa
                    .Include(i => i.BankaHesap)
                    .Where(i => i.Durum == drm)
                    .Where(i => i.BankaHesapId == bankahesapid);
            }

            return bankakasa.
                OrderBy(i => i.Tarih)
                .ToList();

        }

        public List<EBankaKasa> GetAll(int? bankahesapid, bool drm, int page, int pageSize)
        {

            var bankakasa = Context.BankaKasas.AsQueryable();

            if (bankahesapid == null)
            {
                bankakasa = bankakasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.BankaHesap);
            }
            else
            {
                bankakasa = bankakasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.BankaHesap)
                        .Where(i => i.BankaHesapId == bankahesapid);
            }

            return bankakasa
                .OrderBy(i => i.Tarih)
                .Reverse()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(j => j.Tarih)
                .ToList();

        }

        public int GetCount(int? bankahesapid, bool drm)
        {

            var bankakasa = Context.BankaKasas.AsQueryable();

            if (bankahesapid == null)
            {
                bankakasa = bankakasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.BankaHesap);
            }
            else
            {
                bankakasa = bankakasa
                        .Where(i => i.Durum == drm)
                        .Include(i => i.BankaHesap)
                        .Where(i => i.BankaHesapId == bankahesapid);
            }

            return bankakasa.Count();

        }
    }
}
