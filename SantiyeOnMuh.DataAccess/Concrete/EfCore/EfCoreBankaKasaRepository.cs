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
    public class EfCoreBankaKasaRepository : EfCoreGenericRepository<BankaKasa, Context>, IBankaKasaRepository
    {
        public BankaKasa GetByIdDetay(int id)
        {
            using (var context = new Context())
            {
                return context.BankaKasas
                    .Include(i => i.BankaHesap)
                    .FirstOrDefault(i => i.Id == id);
            }
        }

        public List<BankaKasa> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.BankaKasas
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }

        public List<BankaKasa> GetAll(int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var bankakasa = context.BankaKasas.AsQueryable();

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
        }

        public List<BankaKasa> GetAll(int? bankahesapid, bool drm, int page, int pageSize)
        {
            using (var context = new Context())
            {
                var bankakasa = context.BankaKasas.AsQueryable();

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
        }

        public int GetCountByBankaHesap(int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var bankakasa = context.BankaKasas.AsQueryable();

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
}
