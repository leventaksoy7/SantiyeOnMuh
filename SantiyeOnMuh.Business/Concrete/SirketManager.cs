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
    public class SirketManager:ISirketService
    {
        private ISirketRepository _sirketRepository;
        public SirketManager(ISirketRepository sirketRepository)
        {
            _sirketRepository = sirketRepository;
        }

        public void Create(Sirket entity)
        {
            _sirketRepository.Create(entity);
        }

        public void Delete(Sirket entity)
        {
            _sirketRepository.Delete(entity);
        }

        public void Update(Sirket entity)
        {
            _sirketRepository.Update(entity);
        }

        public List<Sirket> GetAll()
        {
            return _sirketRepository.GetAll();
        }

        public List<Sirket> GetAll(bool drm)
        {
            return _sirketRepository.GetAll(drm);
        }

        public Sirket GetById(int id)
        {
            return _sirketRepository.GetById(id);
        }
    }
}
