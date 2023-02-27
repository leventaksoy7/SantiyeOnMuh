﻿using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Abstract
{
    public interface ISantiyeService : IService<Santiye>
    {
        List<Santiye> GetAll(bool drm);
    }
}
