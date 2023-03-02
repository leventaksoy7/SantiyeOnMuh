﻿using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface ISantiyeKasaRepository : IRepository<ESantiyeKasa>
    {
        ESantiyeKasa GetByIdDetay(int id);
        List<ESantiyeKasa> GetAll(int? santiyeid, int? gkid, bool drm);
        List<ESantiyeKasa> GetAll(int santiyeid, int? gkid, bool drm, int page, int pageSize);
        int GetCount(int santiyeid, int? gkid, bool drm);
    }
}
