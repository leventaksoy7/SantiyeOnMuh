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
    public class SantiyeKasaManager : ISantiyeKasaService
    {
        private ISantiyeKasaRepository _santiyeKasaRepository;
        public SantiyeKasaManager (ISantiyeKasaRepository santiyeKasaRepository)
        {
            _santiyeKasaRepository = santiyeKasaRepository;
        }

        public void Create(SantiyeKasa entity)
        {
            _santiyeKasaRepository.Create(entity);
        }

        public void Delete(SantiyeKasa entity)
        {
            _santiyeKasaRepository.Delete(entity);
        }

        public void Update(SantiyeKasa entity)
        {
            _santiyeKasaRepository.Update(entity);
        }

        public List<SantiyeKasa> GetAll()
        {
            return _santiyeKasaRepository.GetAll();
        }

        public SantiyeKasa GetById(int id)
        {
            return _santiyeKasaRepository.GetById(id);
        }
    }
}