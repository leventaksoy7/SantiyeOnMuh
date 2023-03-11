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
                ////DATABASE EKLENMİŞ VERİ KONTROLÜ YAPILIYOR
                //if(context.Santiyes.Count() == 0) 
                //{
                //    //OLUŞTURULAN VERİLERİ İLGİLİ TABLOYA EKLİYOR.
                //    context.Santiyes.AddRange(Santiyes);
                //}

                ////DATABASE EKLENMİŞ VERİ KONTROLÜ YAPILIYOR
                //if (context.SantiyesKasas.Count() == 0)
                //{
                //    //OLUŞTURULAN VERİLERİ İLGİLİ TABLOYA EKLİYOR.
                //    context.SantiyesKasas.AddRange(SantiyeKasas);
                //}

                if (context.CariGiderKalemis.Count() == 0)
                
                { context.CariGiderKalemis.AddRange(CariGiderKalemis); }

                if (context.SantiyeGiderKalemis.Count() == 0)
                
                { context.SantiyeGiderKalemis.AddRange(SantiyeGiderKalemis); }

            }
            context.SaveChanges();
        }

        //private static ESantiye[] Santiyes =
        //{
        //    //new ESantiye(){ Ad="Santiye1"},
        //    //new ESantiye(){ Ad="Santiye2"},
        //    //new ESantiye(){ Ad="Santiye3"}
        //};

        //private static ESantiyeKasa[] SantiyeKasas =
        //{
        //    //new ESantiyeKasa(){ Aciklama="test",Gelir=1,Gider=1,Kisi="test",Tarih=DateTime.Today,SantiyeId=1,No="test",SonGuncelleme=DateTime.Today}
        //};

        private static ECariGiderKalemi[] CariGiderKalemis =
        {
            new ECariGiderKalemi(){Ad="ÇEK",Durum=true,Tur=false},
            new ECariGiderKalemi(){Ad="NAKİT",Durum=true,Tur=false}
        };

        private static ESantiyeGiderKalemi[] SantiyeGiderKalemis =
        {
            new ESantiyeGiderKalemi(){Ad="ŞANTİYE KASASINA EFT",Durum=true,Tur=false},
        };
    }
}
