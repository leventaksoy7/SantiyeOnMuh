using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface IService<T>: IValidator<T>
    {
        T GetById(int id);
        List<T> GetAll();
        bool Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
