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

        public string ErrorMessage { get; set; }

        public bool Validation(ESantiyeGiderKalemi entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Ad))
            {
                ErrorMessage += "GİDER KALEMİ ADI GİRMELİSİNİZ.\n";
                return false;
            }

            return IsValid;
        }

        public bool Create(ESantiyeGiderKalemi entity)
        {
            if (Validation(entity))
            {
                _santiyeGiderKalemiRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(ESantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Delete(entity);
        }

        public void Update(ESantiyeGiderKalemi entity)
        {
            _santiyeGiderKalemiRepository.Update(entity);
        }

        public List<ESantiyeGiderKalemi> GetAll()
        {
            return _santiyeGiderKalemiRepository.GetAll();
        }

        public ESantiyeGiderKalemi GetById(int id)
        {
            return _santiyeGiderKalemiRepository.GetById(id);
        }

        public List<ESantiyeGiderKalemi> GetAll(bool drm, bool tur)
        {
            return _santiyeGiderKalemiRepository.GetAll(drm, tur);
        }

        public List<ESantiyeGiderKalemi> GetAll(bool drm)
        {
            return _santiyeGiderKalemiRepository.GetAll(drm);
        }
    }
}