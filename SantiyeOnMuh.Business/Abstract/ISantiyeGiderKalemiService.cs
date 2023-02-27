﻿using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ISantiyeGiderKalemiService : IService<SantiyeGiderKalemi>
    {
        List<SantiyeGiderKalemi> GetAll(bool drm, bool tur);
    }
}
