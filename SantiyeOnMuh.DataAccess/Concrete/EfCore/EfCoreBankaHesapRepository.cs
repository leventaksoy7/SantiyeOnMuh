﻿using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreBankaHesapRepository : EfCoreGenericRepository<BankaHesap, Context>, IBankaHesapRepository
    {
        public List<BankaHesap> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.BankaHesaps
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
