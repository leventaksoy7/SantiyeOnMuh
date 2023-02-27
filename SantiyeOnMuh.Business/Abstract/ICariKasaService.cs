using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICariKasaService : IService<CariKasa>
    {
        CariKasa GetByIdDetay(int id);
        List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm);
        List<CariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize);
        int GetCountByCariHesapGK(int carihesapid, int? gkid, bool drm);
    }
}
