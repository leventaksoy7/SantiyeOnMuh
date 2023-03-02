using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ICariKasaRepository : IRepository<ECariKasa>, IRepositoryGetAllDurum<ECariKasa>
    {
        ECariKasa GetByIdDetay(int id);
        List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm);
        List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize);
        int GetCount(int carihesapid, int? gkid, bool drm);
    }
}