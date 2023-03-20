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
        //private IBankaHesapRepository _bankaHesapRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BankaHesapManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                _unitOfWork.BankaHesaps.Create(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public void Delete(EBankaHesap entity)
        {
            _unitOfWork.BankaHesaps.Delete(entity);
        }

        public void Update(EBankaHesap entity)
        {
            _unitOfWork.BankaHesaps.Update(entity);
        }

        public List<EBankaHesap> GetAll()
        {
            return _unitOfWork.BankaHesaps.GetAll();
        }

        public List<EBankaHesap> GetAll(bool drm)
        {
            return _unitOfWork.BankaHesaps.GetAll(drm);
        }

        public EBankaHesap GetById(int id)
        {
            return _unitOfWork.BankaHesaps.GetById(id);
        }
    }
}