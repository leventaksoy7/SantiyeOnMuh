﻿using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface IBankaHesapRepository:IRepository<EBankaHesap>, IRepositoryGetAllDurum<EBankaHesap>
    {
    }
}