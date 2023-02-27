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
    public class BankaKasaManager : IBankaKasaService
    {
        private IBankaKasaRepository _bankaKasaRepository;
        public BankaKasaManager (IBankaKasaRepository bankaKasaRepository)
        {
            _bankaKasaRepository = bankaKasaRepository;
        }

        public void Create(BankaKasa entity)
        {
            _bankaKasaRepository.Create(entity);
        }

        public void Update(BankaKasa entity)
        {
            _bankaKasaRepository.Update(entity);
        }

        public void Delete(BankaKasa entity)
        {
            _bankaKasaRepository.Delete(entity);
        }

        public List<BankaKasa> GetAll()
        {
            return _bankaKasaRepository.GetAll();
        }

        public BankaKasa GetById(int id)
        {
            return _bankaKasaRepository.GetById(id);
        }
    }
}
