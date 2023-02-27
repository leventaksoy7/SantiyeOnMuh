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
        public void Create(CariHesap entity)
        {
            _cariHesapRepository.Create(entity);
        }

        public void Delete(CariHesap entity)
        {
            _cariHesapRepository.Delete(entity);
        }

        public void Update(CariHesap entity)
        {
            _cariHesapRepository.Update(entity);
        }

        public List<CariHesap> GetAll()
        {
            return _cariHesapRepository.GetAll();
        }

        public CariHesap GetById(int id)
        {
            return _cariHesapRepository.GetById(id);
        }
    }
}
