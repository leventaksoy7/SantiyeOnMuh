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

        public void Create(CariKasa entity)
        {
            _cariKasaRepository.Create(entity);
        }

        public void Update(CariKasa entity)
        {
            _cariKasaRepository.Update(entity);
        }

        public void Delete(CariKasa entity)
        {
            _cariKasaRepository.Delete(entity);
        }

        public List<CariKasa> GetAll()
        {
            return _cariKasaRepository.GetAll();
        }

        public CariKasa GetById(int id)
        {
            return _cariKasaRepository.GetById(id);
        }
    }
}
