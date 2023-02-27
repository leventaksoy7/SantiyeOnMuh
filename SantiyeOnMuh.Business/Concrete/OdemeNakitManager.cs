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
    public class OdemeNakitManager : IOdemeNakitService
    {
        private IOdemeNakitRepository _odemeNakitRepository;
        public OdemeNakitManager(IOdemeNakitRepository odemeNakitRepository)
        {
            _odemeNakitRepository = odemeNakitRepository;
        }
        public void Create(OdemeNakit entity)
        {
            _odemeNakitRepository.Create(entity);
        }

        public void Delete(OdemeNakit entity)
        {
            _odemeNakitRepository.Delete(entity);
        }

        public void Update(OdemeNakit entity)
        {
            _odemeNakitRepository.Update(entity);
        }

        public List<OdemeNakit> GetAll()
        {
            return _odemeNakitRepository.GetAll();
        }

        public OdemeNakit GetById(int id)
        {
            return _odemeNakitRepository.GetById(id);
        }
    }
}
