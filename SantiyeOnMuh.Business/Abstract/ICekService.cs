using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICekService : IService<ECek>
    {
        ECek GetByIdDetay(int id);
        List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
        List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize);
        int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
    }
}