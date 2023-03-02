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

        public void Create(ESirket entity)
        {
            _sirketRepository.Create(entity);
        }

        public void Delete(ESirket entity)
        {
            _sirketRepository.Delete(entity);
        }

        public void Update(ESirket entity)
        {
            _sirketRepository.Update(entity);
        }

        public List<ESirket> GetAll()
        {
            return _sirketRepository.GetAll();
        }

        public List<ESirket> GetAll(bool drm)
        {
            return _sirketRepository.GetAll(drm);
        }

        public ESirket GetById(int id)
        {
            return _sirketRepository.GetById(id);
        }
    }
}
