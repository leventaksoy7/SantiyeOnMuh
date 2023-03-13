using Microsoft.IdentityModel.Tokens;
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

        public string ErrorMessage { get; set; }

        public bool Validation(EBankaHesap entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.BankaAdi))
            {
                ErrorMessage += "BANKA ADI GİRMELİSİNİZ.\n";
                return false;
            }

            if (string.IsNullOrEmpty(entity.HesapAdi))
            {
                ErrorMessage += "HESAP ADI GİRMELİSİNİZ.\n";
                return false;
            }

            return IsValid;
        }

        public bool Create(EBankaHesap entity)
        {
            if (Validation(entity))
            {
                _bankaHesapRepository.Create(entity);
                return true;
            }
            return false;
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