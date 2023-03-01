using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICekService : IService<Cek>
    {
        Cek GetByIdDetay(int id);
        List<Cek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
        List<Cek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize);
        int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
    }
}