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
    public class CekManager : ICekService
    {
        private ICekRepository _cekRepository;
        public CekManager(ICekRepository CekRepository)
        {
            _cekRepository = CekRepository;
        }
        public void Create(Cek entity)
        {
            _cekRepository.Create(entity);
        }

        public void Delete(Cek entity)
        {
            _cekRepository.Delete(entity);
        }

        public void Update(Cek entity)
        {
            _cekRepository.Update(entity);
        }

        public List<Cek> GetAll()
        {
            return _cekRepository.GetAll();
        }

        public Cek GetById(int id)
        {
            return _cekRepository.GetById(id);
        }

        public Cek GetByIdDetay(int id)
        {
            return _cekRepository.GetByIdDetay(id);
        }

        public List<Cek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _cekRepository.GetAll(santiyeid, sirketid, bankahesapid,drm);
        }

        public List<Cek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _cekRepository.GetAll(santiyeid,sirketid,bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _cekRepository.GetCount(santiyeid, sirketid, bankahesapid, drm);
        }
    }
}
