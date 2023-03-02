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
    public class CariGiderKalemiManager : ICariGiderKalemiService
    {
        private ICariGiderKalemiRepository _cariGiderKalemiRepository;
        public CariGiderKalemiManager(ICariGiderKalemiRepository cariGiderKalemiRepository)
        {
            _cariGiderKalemiRepository = cariGiderKalemiRepository;
        }

        public void Create(ECariGiderKalemi entity)
        {
            _cariGiderKalemiRepository.Create(entity);
        }

        public void Delete(ECariGiderKalemi entity)
        {
            _cariGiderKalemiRepository.Delete(entity);
        }

        public void Update(ECariGiderKalemi entity)
        {
            _cariGiderKalemiRepository.Update(entity);
        }

        public ECariGiderKalemi GetById(int id)
        {
            return _cariGiderKalemiRepository.GetById(id);
        }

        public List<ECariGiderKalemi> GetAll()
        {
            return _cariGiderKalemiRepository.GetAll();
        }

        public List<ECariGiderKalemi> GetAll(bool drm, bool tur)
        {
            return _cariGiderKalemiRepository.GetAll(drm, tur);
        }

        public List<ECariGiderKalemi> GetAll(bool drm)
        {
            return _cariGiderKalemiRepository.GetAll(drm);
        }
    }
}
