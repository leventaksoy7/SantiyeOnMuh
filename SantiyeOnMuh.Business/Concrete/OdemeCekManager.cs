using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Concrete
{
    public class OdemeCekManager : IOdemeCekService
    {
        private IOdemeCekRepository _odemeCekRepository;
        public OdemeCekManager(IOdemeCekRepository odemeCekRepository)
        {
            _odemeCekRepository = odemeCekRepository;
        }
        public void Create(OdemeCek entity)
        {
            _odemeCekRepository.Create(entity);
        }

        public void Delete(OdemeCek entity)
        {
            _odemeCekRepository.Delete(entity);
        }

        public void Update(OdemeCek entity)
        {
            _odemeCekRepository.Update(entity);
        }

        public List<OdemeCek> GetAll()
        {
            return _odemeCekRepository.GetAll();
        }

        public OdemeCek GetById(int id)
        {
            return _odemeCekRepository.GetById(id);
        }

        public OdemeCek GetByIdDetay(int id)
        {
            return _odemeCekRepository.GetByIdDetay(id);
        }

        public List<OdemeCek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _odemeCekRepository.GetAll(santiyeid, sirketid, bankahesapid,drm);
        }

        public List<OdemeCek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _odemeCekRepository.GetAll(santiyeid,sirketid,bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _odemeCekRepository.GetCount(santiyeid, sirketid, bankahesapid, drm);
        }
    }
}
