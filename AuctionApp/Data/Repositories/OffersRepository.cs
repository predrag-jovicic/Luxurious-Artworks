using AuctionApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repositories
{
    public class OffersRepository
    {
        private AuctionDbContext _context;

        public OffersRepository(AuctionDbContext context)
        {
            _context = context;
        }

        #region Get methods
        public IEnumerable<Offer> GetMyOffers(string userId, int numberToSkip = 0)
        {
            return _context.Offers
                .Include(o => o.User)
                .Include(o => o.Auction.ArtWork)
                .Include(o => o.Auction.ArtWork.Author)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.DateTime)
                .Skip(numberToSkip)
                .ToList();
        }
        public double GetHighestBid(int auct)
        {
            if (IsThereAnyOffer(auct))
                return _context.Offers.Where(off => off.AuctionId == auct).Select(off => off.Amount).Max();
            else
                return 0;
        }
        public IEnumerable<Offer> GetLatestBidsForParticularAuction(int auct, int numberOfRows, int numberToSkip = 0)
        {
            return _context.Offers
                .Include(o => o.User)
                .Where(o => o.AuctionId == auct)
                .OrderByDescending(o => o.DateTime)
                .Skip(numberToSkip)
                .Take(numberOfRows)
                .ToList();
        }
        #endregion
        #region Count methods
        public int AuctionOffersCount(string userId, int auctionId)
        {
            return _context.Offers
                .Where(offer => offer.AuctionId == auctionId && offer.Auction.UserId == userId)
                .Count();
        }
        public int MyBidsCount(string userId)
        {
            return _context.Offers.
                Where(offer => userId == offer.UserId)
                .Count();
        } 
        public bool IsThereAnyOffer(int auctionId)
        {
            return _context.Offers.Where(a => a.AuctionId == auctionId).Any();
        }
        #endregion
        #region Add method
        public void Add(Offer newOffer)
        {
            _context.Offers.Add(newOffer);
        }
        #endregion
        #region DeleteMethod
        public void DeleteOffers(int auctionId)
        {
            var offers = this._context.Offers.Where(o => o.AuctionId == auctionId);
            if (offers.Any())
                this._context.Offers.RemoveRange(offers);
        } 
        #endregion
    }
}
