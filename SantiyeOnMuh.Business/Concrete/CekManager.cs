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
    public class CekManager : ICekService
    {
        private ICekRepository _cekRepository;
        public CekManager(ICekRepository CekRepository)
        {
            _cekRepository = CekRepository;
        }
        public void Create(ECek entity)
        {
            _cekRepository.Create(entity);
        }

        public void Delete(ECek entity)
        {
            _cekRepository.Delete(entity);
        }

        public void Update(ECek entity)
        {
            _cekRepository.Update(entity);
        }

        public List<ECek> GetAll()
        {
            return _cekRepository.GetAll();
        }

        public ECek GetById(int id)
        {
            return _cekRepository.GetById(id);
        }

        public ECek GetByIdDetay(int id)
        {
            return _cekRepository.GetByIdDetay(id);
        }

        public List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _cekRepository.GetAll(santiyeid, sirketid, bankahesapid,drm);
        }

        public List<ECek> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _cekRepository.GetAll(santiyeid,sirketid,bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _cekRepository.GetCount(santiyeid, sirketid, bankahesapid, drm);
        }
    }
}
