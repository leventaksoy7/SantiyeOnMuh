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
        public void Create(ENakit entity)
        {
            _nakitRepository.Create(entity);
        }

        public void Delete(ENakit entity)
        {
            _nakitRepository.Delete(entity);
        }

        public void Update(ENakit entity)
        {
            _nakitRepository.Update(entity);
        }

        public List<ENakit> GetAll()
        {
            return _nakitRepository.GetAll();
        }

        public ENakit GetById(int id)
        {
            return _nakitRepository.GetById(id);
        }

        public ENakit GetByIdDetay(int id)
        {
            return _nakitRepository.GetByIdDetay(id);
        }

        public List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _nakitRepository.GetAll(santiyeid, sirketid, bankahesapid, drm);
        }

        public List<ENakit> GetAll(int? santiyeid, int? sirketid, int? bankahesapid, bool drm, int page, int pageSize)
        {
            return _nakitRepository.GetAll(santiyeid,sirketid,bankahesapid,drm,page,pageSize);
        }

        public int GetCount(int? santiyeid, int? sirketid, int? bankahesapid, bool drm)
        {
            return _nakitRepository.GetCount(santiyeid,sirketid,bankahesapid, drm);
        }
    }
}
