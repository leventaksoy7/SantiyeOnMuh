using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    internal class EfCoreBankaKasaRepository:EfCoreGenericRepository<BankaKasa,Context>,IBankaKasaRepository
    {
    }
}
