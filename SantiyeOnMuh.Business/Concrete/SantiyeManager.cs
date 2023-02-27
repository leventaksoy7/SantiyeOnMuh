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
        //CRUD
        public void Create(Santiye entity)
        {
            //İŞ KURALLARI
            _santiyeRepository.Create(entity);
        }

        public void Update(Santiye entity)
        {
            _santiyeRepository.Update(entity);
        }

        public void Delete(Santiye entity)
        {
            _santiyeRepository.Delete(entity);
        }

        public Santiye GetById(int id)
        {
            return _santiyeRepository.GetById(id);
        }

        public List<Santiye> GetAll()
        {
            return _santiyeRepository.GetAll();
        }
    }
}
