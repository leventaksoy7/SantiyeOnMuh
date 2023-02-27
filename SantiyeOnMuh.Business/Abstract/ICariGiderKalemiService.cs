using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ICariGiderKalemiService : IService<CariGiderKalemi>,IServiceGetAllDurum<CariGiderKalemi>
    {
        public List<CariGiderKalemi> GetAll(bool drm, bool tur);
    }
}