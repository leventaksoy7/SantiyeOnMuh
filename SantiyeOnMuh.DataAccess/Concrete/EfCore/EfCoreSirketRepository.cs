using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    internal class EfCoreSirketRepository:EfCoreGenericRepository<Sirket,Context>, ISirketRepository
    {
    }
}
