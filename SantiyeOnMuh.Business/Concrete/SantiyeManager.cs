using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;
using SantiyeOnMuh.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Concrete
{
    public class SantiyeManager : ISantiyeService
    {
        //BAĞIMLILIĞI ORTADAN KALDIRMAK İÇİN INTERFACE ÜZERİNDEN İŞLEM YAPIYORUM
        private ISantiyeRepository _santiyeRepository;
        //INJECTION
        public SantiyeManager(ISantiyeRepository santiyeRepository)
        {
            _santiyeRepository = santiyeRepository;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(ESantiye entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Ad))
            {
                ErrorMessage += "ŞANTİYE ADI GİRMELİSİNİZ.\n";
                return false;
            }

            return IsValid;
        }
        //CRUD
        public bool Create(ESantiye entity)
        {
            //İŞ KURALLARI
            if (Validation(entity))
            {
                _santiyeRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Update(ESantiye entity)
        {
            _santiyeRepository.Update(entity);
        }

        public void Delete(ESantiye entity)
        {
            _santiyeRepository.Delete(entity);
        }

        public ESantiye GetById(int id)
        {
            return _santiyeRepository.GetById(id);
        }

        public List<ESantiye> GetAll()
        {
            return _santiyeRepository.GetAll();
        }

        public List<ESantiye> GetAll(bool drm)
        {
            return _santiyeRepository.GetAll(drm);
        }
    }
}
