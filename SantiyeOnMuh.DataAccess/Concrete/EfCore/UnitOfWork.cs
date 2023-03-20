using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public UnitOfWork(Context context)
        {
            _context = context;
        }

        private EfCoreBankaHesapRepository _bankaHesapRepository { get; }
        private EfCoreBankaKasaRepository _bankaKasaRepository { get; }
        private EfCoreCariGiderKalemiRepository _cariGiderKalemiRepository { get; }
        private EfCoreCariHesapRepository _cariHesapRepository { get; }
        private EfCoreCariKasaRepository _cariKasaRepository { get; }
        private EfCoreCekRepository _cekRepository { get; }
        private EfCoreNakitRepository _nakitRepository { get; }
        private EfCoreSantiyeGiderKalemiRepository _santiyeGiderKalemiRepository { get; }
        private EfCoreSantiyeKasaRepository _santiyeKasaRepository { get; }
        private EfCoreSantiyeRepository _santiyeRepository { get; }
        private EfCoreSirketRepository _sirketRepository { get; }




        public IBankaHesapRepository BankaHesaps => _bankaHesapRepository ?? new EfCoreBankaHesapRepository(_context);

        public IBankaKasaRepository BankaKasas => _bankaKasaRepository ?? new EfCoreBankaKasaRepository(_context);

        public ICariGiderKalemiRepository CariGiderKalemis => _cariGiderKalemiRepository ?? new EfCoreCariGiderKalemiRepository(_context);

        public ICariHesapRepository CariHesaps => _cariHesapRepository ?? new EfCoreCariHesapRepository(_context);

        public ICariKasaRepository CariKasas => _cariKasaRepository ?? new EfCoreCariKasaRepository(_context);

        public ICekRepository Ceks => _cekRepository ?? new EfCoreCekRepository(_context);

        public INakitRepository Nakits => _nakitRepository ?? new EfCoreNakitRepository(_context);

        public ISantiyeGiderKalemiRepository SantiyeGiderKalemis => _santiyeGiderKalemiRepository ?? new EfCoreSantiyeGiderKalemiRepository(_context);

        public ISantiyeKasaRepository SantiyeKasas => _santiyeKasaRepository ?? new EfCoreSantiyeKasaRepository(_context);

        public ISantiyeRepository Santiyes => _santiyeRepository ?? new EfCoreSantiyeRepository(_context);

        public ISirketRepository Sirkets => _sirketRepository ?? new EfCoreSirketRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
