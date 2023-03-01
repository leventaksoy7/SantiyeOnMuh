using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Business.Concrete
{
    public class NakitManager : INakitService
    {
        private INakitRepository _nakitRepository;
        public NakitManager(INakitRepository NakitRepository)
        {
            _nakitRepository = NakitRepository;
        }
        public void Create(Nakit entity)
        {
            _nakitRepository.Create(entity);
        }

        public void Delete(Nakit entity)
        {
            _nakitRepository.Delete(entity);
        }

        public void Update(Nakit entity)
        {
            _nakitRepository.Update(entity);
        }

        public List<Nakit> GetAll()
        {
            return _nakitRepository.GetAll();
        }

        public Nakit GetById(int id)
        {
            return _nakitRepository.GetById(id);
        }

        public Nakit GetByIdDetay(int id)
        {
            return _nakitRepository.GetByIdDetay(id);
        }

        public List<Nakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _nakitRepository.GetAll(santiyeid, sirketid, bankahesapid, drm);
        }

        public List<Nakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _nakitRepository.GetAll(santiyeid,sirketid,bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _nakitRepository.GetCount(santiyeid,sirketid,bankahesapid, drm);
        }
    }
}
