using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class MyAuctionsViewModel
    {
        public int AuctionId { get; set; }
        public double StartingPrice { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Author { get; set; }
        public bool Sold { get; set; }
        public double HighestBid { get; set; }
    }
}
