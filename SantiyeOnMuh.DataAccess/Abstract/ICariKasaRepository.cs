using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ICariKasaRepository : IRepository<CariKasa>, IRepositoryGetAllDurum<CariKasa>
    {
        CariKasa GetByIdDetay(int id);
        List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm);
        List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize);
        int GetCount(int carihesapid, int? gkid, bool drm);
    }
}