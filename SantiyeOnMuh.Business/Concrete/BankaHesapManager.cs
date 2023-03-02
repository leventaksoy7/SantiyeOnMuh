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
    public class BankaHesapManager : IBankaHesapService
    {
        private IBankaHesapRepository _bankaHesapRepository;
        public BankaHesapManager(IBankaHesapRepository bankaHesapRepository)
        {
            _bankaHesapRepository = bankaHesapRepository;
        }

        public void Create(EBankaHesap entity)
        {
            _bankaHesapRepository.Create(entity);
        }

        public void Delete(EBankaHesap entity)
        {
            _bankaHesapRepository.Delete(entity);
        }

        public void Update(EBankaHesap entity)
        {
            _bankaHesapRepository.Update(entity);
        }

        public List<EBankaHesap> GetAll()
        {
            return _bankaHesapRepository.GetAll();
        }

        public List<EBankaHesap> GetAll(bool drm)
        {
            return _bankaHesapRepository.GetAll(drm);
        }

        public EBankaHesap GetById(int id)
        {
            return _bankaHesapRepository.GetById(id);
        }
    }
}