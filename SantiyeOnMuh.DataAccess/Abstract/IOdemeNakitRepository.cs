using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface IOdemeNakitRepository:IRepository<OdemeNakit>, IRepositoryGetAllDurum<OdemeNakit>
    {
        OdemeNakit GetByIdDetay(int id);
        List<OdemeNakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
        List<OdemeNakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize);
        int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
    }
}
