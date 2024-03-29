﻿using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface IBankaKasaRepository:IRepository<EBankaKasa>,IRepositoryGetAllDurum<EBankaKasa>
    {
        EBankaKasa GetByIdDetay(int id);
        List<EBankaKasa> GetAll(int? bankahesapid, bool drm);
        List<EBankaKasa> GetAll(int? bankahesapid, bool drm, int page, int pageSize);
        int GetCount(int? bankahesapid, bool drm);
    }
}
