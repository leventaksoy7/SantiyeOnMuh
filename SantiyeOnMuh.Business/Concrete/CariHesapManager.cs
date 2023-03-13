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
    public class CariHesapManager : ICariHesapService
    {
        private ICariHesapRepository _cariHesapRepository;

        public CariHesapManager (ICariHesapRepository cariHesapRepository)
        {
            _cariHesapRepository = cariHesapRepository;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(ECariHesap entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Ad))
            {
                ErrorMessage += "GİDER KALEMİ ADI GİRMELİSİNİZ.\n";
                return false;
            }

            if (string.IsNullOrEmpty(entity.Telefon))
            {
                if (Validation(entity))
                {
                    ErrorMessage += "TELEFON NUMARASI GİRMELİSİNİZ.\n";
                    return false;
                }
                return false;
            }

            if (string.IsNullOrEmpty(entity.IlgiliKisi))
            {
                ErrorMessage += "YETKİLİ KİŞİ GİRMELİSİNİZ.\n";
                return false;
            }

            if (string.IsNullOrEmpty(entity.IlgiliKisiTelefon))
            {
                ErrorMessage += "TELEFON NUMARASI GİRMELİSİNİZ.\n";
                return false;
            }

            return IsValid;
        }

        public bool Create(ECariHesap entity)
        {
            _cariHesapRepository.Create(entity);
            return true;
        }

        public void Delete(ECariHesap entity)
        {
            _cariHesapRepository.Delete(entity);
        }

        public void Update(ECariHesap entity)
        {
            _cariHesapRepository.Update(entity);
        }

        public List<ECariHesap> GetAll()
        {
            return _cariHesapRepository.GetAll();
        }

        public ECariHesap GetById(int id)
        {
            return _cariHesapRepository.GetById(id);
        }

        public List<ECariHesap> GetAll(int? santiyeid, bool drm)
        {
            return _cariHesapRepository.GetAll(santiyeid, drm);
        }

        public List<ECariHesap> GetAll(int? santiyeid, bool drm, int page, int pageSize)
        {
            return _cariHesapRepository.GetAll(santiyeid, drm, page, pageSize);
        }

        public int GetCount(int? santiyeid, bool drm)
        {
            return _cariHesapRepository.GetCount(santiyeid, drm);
        }
    }
}