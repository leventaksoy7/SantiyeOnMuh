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

        public string ErrorMessage { get; set; }

        public bool Validation(ECariKasa entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Aciklama))
            {
                ErrorMessage += "AÇIKLAMA GİRMELİSİNİZ.\n";
                return false;
            }

            if (entity.Miktar<0)
            {
                ErrorMessage += "MİKTAR -0- DAN KÜÇÜK OLAMAZ.\n";
                return false;
            }

            if (entity.BirimFiyat<0)
            {
                ErrorMessage += "BİRİM FİYAT -0- DAN KÜÇÜK OLAMAZ.\n";
                return false;
            }

            if (entity.Borc < 0)
            {
                ErrorMessage += "BORÇ -0- DAN KÜÇÜK OLAMAZ.\n";
                return false;
            }

            if (entity.Alacak < 0)
            {
                ErrorMessage += "BORÇ -0- DAN KÜÇÜK OLAMAZ.\n";
                return false;
            }

            return IsValid;
        }

        public bool Create(ECariKasa entity)
        {
            if (Validation(entity))
            {
                _cariKasaRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Update(ECariKasa entity)
        {
            _cariKasaRepository.Update(entity);
        }

        public void Delete(ECariKasa entity)
        {
            _cariKasaRepository.Delete(entity);
        }

        public List<ECariKasa> GetAll()
        {
            return _cariKasaRepository.GetAll();
        }

        public ECariKasa GetById(int id)
        {
            return _cariKasaRepository.GetById(id);
        }

        public ECariKasa GetByIdDetay(int id)
        {
            return _cariKasaRepository.GetByIdDetay(id);
        }

        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm)
        {
            return _cariKasaRepository.GetAll(carihesapid, gkid, drm);
        }

        public List<ECariKasa> GetAll(int carihesapid, int? gkid, bool drm, int page, int pageSize)
        {
            return _cariKasaRepository.GetAll(carihesapid,gkid, drm, page, pageSize);
        }

        public int GetCount(int carihesapid, int? gkid, bool drm)
        {
            return _cariKasaRepository.GetCount(carihesapid,gkid,drm);
        }
    }
}