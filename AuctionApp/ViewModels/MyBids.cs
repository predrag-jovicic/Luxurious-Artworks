using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class MyBids
    {
        public string ArtWorkName { get; set; }
        public string Author { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
        public int AuctionId { get; set; }
    }
}
