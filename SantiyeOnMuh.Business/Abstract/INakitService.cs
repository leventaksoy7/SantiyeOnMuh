using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface INakitService : IService<ENakit>
    {
        ENakit GetByIdDetay(int id);
        List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
        List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize);
        int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm);
    }
}