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

        public string ErrorMessage { get; set; }

        public bool Validation(EBankaKasa entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Aciklama))
            {
                ErrorMessage += "AÇIKLAMA GİRMELİSİNİZ.\n";
                return false;
            }

            if (string.IsNullOrEmpty(entity.Nitelik))
            {
                ErrorMessage += "NİTELİK GİRMELİSİNİZ.\n";
                return false;
            }

            if ((entity.Giren<0) || (entity.Cikan < 0))
            {
                ErrorMessage += "TUTAR -0- DAN KÜÇÜK OLAMAZ .\n";
                return false;
            }

            return IsValid;
        }

        public bool Create(EBankaKasa entity)
        {
            if (Validation(entity))
            {
                _bankaKasaRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Update(EBankaKasa entity)
        {
            _bankaKasaRepository.Update(entity);
        }

        public void Delete(EBankaKasa entity)
        {
            _bankaKasaRepository.Delete(entity);
        }

        public List<EBankaKasa> GetAll()
        {
            return _bankaKasaRepository.GetAll();
        }

        public EBankaKasa GetById(int id)
        {
            return _bankaKasaRepository.GetById(id);
        }

        public EBankaKasa GetByIdDetay(int id)
        {
            return _bankaKasaRepository.GetById(id);
        }

        public List<EBankaKasa> GetAll(int? bankahesapid, bool drm)
        {
            return _bankaKasaRepository.GetAll(bankahesapid, drm);
        }

        public List<EBankaKasa> GetAll(int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _bankaKasaRepository.GetAll(bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? bankahesapid, bool drm)
        {
            return _bankaKasaRepository.GetCount(bankahesapid, drm);
        }
    }
}
