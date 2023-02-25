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

        public void Create(Santiye entity)
        {
            //İŞ KURALLARI
            //_santiyeRepository.Create(entity);
        }

        public void Update(Santiye entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Santiye entity)
        {
            throw new NotImplementedException();
        }

        public List<Santiye> GetAll()
        {
            throw new NotImplementedException();
        }

        public Santiye GetById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
