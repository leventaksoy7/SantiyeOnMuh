using Microsoft.EntityFrameworkCore;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new Context();

            //TUM MIGRATIONLARIN UYGULANDIĞINI KONTROL EDILIYOR
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                //DATABASE EKLENMİŞ VERİ KONTROLÜ YAPILIYOR
                if(context.Santiyes.Count() == 0) 
                {
                    //OLUŞTURULAN VERİLERİ İLGİLİ TABLOYA EKLİYOR.
                    context.Santiyes.AddRange(Santiyes);
                }

                //DATABASE EKLENMİŞ VERİ KONTROLÜ YAPILIYOR
                if (context.SantiyesKasa.Count() == 0)
                {
                    //OLUŞTURULAN VERİLERİ İLGİLİ TABLOYA EKLİYOR.
                    context.SantiyesKasa.AddRange(SantiyeKasas);
                }
            }
            context.SaveChanges();
        }

        private static Santiye[] Santiyes =
        {
            new Santiye(){ Ad="Santiye1"},
            new Santiye(){ Ad="Santiye2"},
            new Santiye(){ Ad="Santiye3"}
        };

        private static SantiyeKasa[] SantiyeKasas =
        {
            new SantiyeKasa(){ Aciklama="test",Gelir=1,Gider=1,Kisi="test",Tarih=DateTime.Today,SantiyeId=1,No="test",SonGuncelleme=DateTime.Today}
        };
    }
}
