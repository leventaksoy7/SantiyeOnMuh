﻿using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Concrete
{
    public class SantiyeKasaManager : ISantiyeKasaService
    {
        private ISantiyeKasaRepository _santiyeKasaRepository;
        public SantiyeKasaManager (ISantiyeKasaRepository santiyeKasaRepository)
        {
            _santiyeKasaRepository = santiyeKasaRepository;
        }

        public void Create(ESantiyeKasa entity)
        {
            _santiyeKasaRepository.Create(entity);
        }

        public void Delete(ESantiyeKasa entity)
        {
            _santiyeKasaRepository.Delete(entity);
        }

        public void Update(ESantiyeKasa entity)
        {
            _santiyeKasaRepository.Update(entity);
        }

        public List<ESantiyeKasa> GetAll()
        {
            return _santiyeKasaRepository.GetAll();
        }

        public ESantiyeKasa GetById(int id)
        {
            return _santiyeKasaRepository.GetById(id);
        }

        public List<ESantiyeKasa> GetAll(int? santiyeid, int? gkid, bool drm)
        {
            return _santiyeKasaRepository.GetAll(santiyeid,gkid,drm);
        }

        public ESantiyeKasa GetByIdDetay(int id)
        {
            return _santiyeKasaRepository.GetByIdDetay(id);
        }

        public List<ESantiyeKasa> GetAll(int santiyeid, int? gkid, bool drm, int page, int pageSize)
        {
            return _santiyeKasaRepository.GetAll(santiyeid, gkid, drm, page, pageSize);
        }

        public int GetCount(int santiyeid, int? gkid, bool drm)
        {
            return _santiyeKasaRepository.GetCount(santiyeid, gkid, drm);
        }
    }
}