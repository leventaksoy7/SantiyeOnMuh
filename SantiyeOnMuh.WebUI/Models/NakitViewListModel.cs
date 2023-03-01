﻿using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class NakitViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<BankaHesap> BankaHesaps { get; set; }
        public List<Santiye> Santiyes { get; set; }
        public List<Sirket> Sirkets { get; set; }
        public List<Nakit> Nakits { get; set; }
    }
}