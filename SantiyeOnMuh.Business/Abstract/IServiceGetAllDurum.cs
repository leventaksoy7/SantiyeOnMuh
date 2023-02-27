using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface IServiceGetAllDurum<T> where T : class
    {
        List<T> GetAll(bool drm);
    }
}
