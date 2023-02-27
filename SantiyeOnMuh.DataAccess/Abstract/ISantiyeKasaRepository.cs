using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ISantiyeKasaRepository : IRepository<SantiyeKasa>
    {
        SantiyeKasa GetByIdDetay(int id);
        List<SantiyeKasa> GetAll(int? santiyeid, int? gkid, bool drm);
        List<SantiyeKasa> GetAll(int santiyeid, int? gkid, bool drm, int page, int pageSize);
        int GetCountByGiderKalemi(int santiyeid, int? gkid, bool drm);
    }
}
