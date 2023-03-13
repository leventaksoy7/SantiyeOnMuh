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
    public class SirketManager:ISirketService
    {
        private ISirketRepository _sirketRepository;

        public SirketManager(ISirketRepository sirketRepository)
        {
            _sirketRepository = sirketRepository;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(ESirket entity)
        {
            var IsValid = true;

            if (string.IsNullOrEmpty(entity.Ad))
            {
                ErrorMessage += "ŞİRKET ADI GİRMELİSİNİZ.\n";
                return false;
            }


            return IsValid;
        }

        public bool Create(ESirket entity)
        {
            if (Validation(entity))
            {
                _sirketRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(ESirket entity)
        {
            _sirketRepository.Delete(entity);
        }

        public void Update(ESirket entity)
        {
            _sirketRepository.Update(entity);
        }

        public List<ESirket> GetAll()
        {
            return _sirketRepository.GetAll();
        }

        public List<ESirket> GetAll(bool drm)
        {
            return _sirketRepository.GetAll(drm);
        }

        public ESirket GetById(int id)
        {
            return _sirketRepository.GetById(id);
        }

    }
}
