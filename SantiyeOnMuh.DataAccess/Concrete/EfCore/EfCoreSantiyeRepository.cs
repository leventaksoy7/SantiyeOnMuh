using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.EfCore;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.SqlServer
{
    internal class EfCoreSantiyeRepository : EfCoreGenericRepository<Santiye,Context>,IRepository<Santiye>
    {
    }
}
