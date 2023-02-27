using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICariHesapService : IService<CariHesap>
    {
        List<CariHesap> GetAll(int? santiyeid, bool drm);
        List<CariHesap> GetAll(int? santiyeid, bool drm, int page, int pageSize);
        int GetCountBySantiye(int? santiyeid, bool drm);
    }
}