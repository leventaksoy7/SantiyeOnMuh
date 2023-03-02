using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ISantiyeGiderKalemiRepository : IRepository<ESantiyeGiderKalemi>, IRepositoryGetAllDurum<ESantiyeGiderKalemi>
    {
        //EKSTRA METODLAR EKLENECEKSE BURAYA
        List<ESantiyeGiderKalemi> GetAll(bool drm, bool tur);
    }
}
