using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ICariGiderKalemiRepository : IRepository<CariGiderKalemi>, IRepositoryGetAllDurum<CariGiderKalemi>
    {
        public List<CariGiderKalemi> GetAll(bool drm,bool tur);
    }
}
