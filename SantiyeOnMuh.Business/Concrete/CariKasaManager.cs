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
    public class CariKasaManager : ICariKasaService
    {
        private ICariKasaRepository _cariKasaRepository;
        public CariKasaManager(ICariKasaRepository cariKasaRepository)
        {
            _cariKasaRepository = cariKasaRepository;
        }

        public void Create(ECariKasa entity)
        {
            _cariKasaRepository.Create(entity);
        }

        public void Update(ECariKasa entity)
        {
            _cariKasaRepository.Update(entity);
        }

        public void Delete(ECariKasa entity)
        {
            _cariKasaRepository.Delete(entity);
        }

        public List<ECariKasa> GetAll()
        {
            return _cariKasaRepository.GetAll();
        }

        public ECariKasa GetById(int id)
        {
            return _cariKasaRepository.GetById(id);
        }

        public ECariKasa GetByIdDetay(int id)
        {
            return _cariKasaRepository.GetByIdDetay(id);
        }

        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm)
        {
            return _cariKasaRepository.GetAll(carihesapid, gkid, drm);
        }

        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize)
        {
            return _cariKasaRepository.GetAll(carihesapid,gkid, drm, page, pageSize);
        }

        public int GetCount(int carihesapid, int? gkid, bool drm)
        {
            return _cariKasaRepository.GetCount(carihesapid,gkid,drm);
        }
    }
}