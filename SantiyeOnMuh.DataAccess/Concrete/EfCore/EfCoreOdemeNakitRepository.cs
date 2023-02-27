using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreOdemeNakitRepository : EfCoreGenericRepository<OdemeNakit, Context>, IOdemeNakitRepository
    {
        public List<OdemeNakit> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.OdemeNakits
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
