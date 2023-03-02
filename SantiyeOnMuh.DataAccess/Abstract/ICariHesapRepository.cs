using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ICariHesapRepository : IRepository<ECariHesap>, IRepositoryGetAllDurum<ECariHesap>
    {
        List<ECariHesap> GetAll(int? santiyeid, bool drm);
        List<ECariHesap> GetAll(int? santiyeid, bool drm, int page, int pageSize);
        int GetCount(int? santiyeid, bool drm);
    }
}
