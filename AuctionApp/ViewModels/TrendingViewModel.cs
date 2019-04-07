using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class TrendingViewModel
    {
        public Author Author { get; set; }
        public Auction Auction { get; set; }
    }
}
