using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class LatestOffersViewModel
    {
        public IEnumerable<Offer> LatestBids { get; set; }
        public int AuctionId { get; set; }
        public string UserCreated { get; set; }
        public string CurrentUser { get; set; }
    }
}
