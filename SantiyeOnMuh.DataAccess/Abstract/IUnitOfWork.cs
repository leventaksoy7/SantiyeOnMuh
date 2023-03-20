using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        IBankaHesapRepository BankaHesaps { get; }
        IBankaKasaRepository BankaKasas { get; }
        ICariGiderKalemiRepository CariGiderKalemis { get; }
        ICariHesapRepository CariHesaps { get; }
        ICariKasaRepository CariKasas { get; }
        ICekRepository Ceks { get; }
        INakitRepository Nakits { get; }
        ISantiyeGiderKalemiRepository SantiyeGiderKalemis { get; }
        ISantiyeKasaRepository SantiyeKasas { get; }
        ISantiyeRepository Santiyes { get; }
        ISirketRepository Sirkets { get; }
        void Save();
    }
}
