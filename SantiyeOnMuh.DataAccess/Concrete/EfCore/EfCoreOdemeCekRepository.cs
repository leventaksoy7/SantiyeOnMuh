﻿using Microsoft.EntityFrameworkCore;
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
    public class EfCoreOdemeCekRepository : EfCoreGenericRepository<OdemeCek, Context>, IOdemeCekRepository
    {
        public List<OdemeCek> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.OdemeCeks
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }
        public OdemeCek GetByIdDetay(int id)
        {
            using (var context = new Context())
            {
                return context.OdemeCeks.
                     Include(y => y.Sirket)
                    .Include(x => x.CariHesap)
                    .Include(x => x.BankaHesap)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        public List<OdemeCek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var cek = context.OdemeCeks.AsQueryable();

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
        }
        public List<OdemeCek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            using (var context = new Context())
            {
                var cek = context.OdemeCeks.AsQueryable();

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
        }
        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            using (var context = new Context())
            {
                var cek = context.OdemeCeks.AsQueryable();

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
}
