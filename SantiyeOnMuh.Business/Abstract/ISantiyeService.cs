using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ISantiyeService
    {
        Santiye GetById(int id);
        List<Santiye> GetAll();
        void Create(Santiye entity);
        void Update(Santiye entity);
        void Delete(Santiye entity);
    }
}
