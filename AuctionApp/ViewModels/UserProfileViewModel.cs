using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public IEnumerable<Offer> MyBids { get; set; }
        public IEnumerable<Auction> MyAuctions { get; set; }
    }
}
