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
    public class SantiyeGiderKalemiManager : ISantiyeGiderKalemiService
    {
        private ISantiyeGiderKalemiRepository _santiyeGiderKalemiRepository;
        public SantiyeGiderKalemiManager (ISantiyeGiderKalemiRepository santiyeGiderKalemiRepositoryservice)
        {
            _santiyeGiderKalemiRepository = santiyeGiderKalemiRepositoryservice;
        }
        public void Create(ESantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Create(entity);
        }

        public void Delete(ESantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Delete(entity);
        }

        public void Update(ESantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Update(entity);
        }

        public List<ESantiyeGiderKalemi> GetAll()
        {
            return _santiyeGiderKalemiRepository.GetAll();
        }

        public ESantiyeGiderKalemi GetById(int id)
        {
            return _santiyeGiderKalemiRepository.GetById(id);
        }

        public List<ESantiyeGiderKalemi> GetAll(bool drm, bool tur)
        {
            return _santiyeGiderKalemiRepository.GetAll(drm, tur);
        }
    }
}