using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICariGiderKalemiService : IService<ECariGiderKalemi>,IServiceGetAllDurum<ECariGiderKalemi>
    {
        public List<ECariGiderKalemi> GetAll(bool drm, bool tur);
    }
}