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
        public void Create(SantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Create(entity);
        }

        public void Delete(SantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Delete(entity);
        }

        public void Update(SantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Update(entity);
        }

        public List<SantiyeGiderKalemi> GetAll()
        {
            return _santiyeGiderKalemiRepository.GetAll();
        }

        public SantiyeGiderKalemi GetById(int id)
        {
            return _santiyeGiderKalemiRepository.GetById(id);
        }
    }
}