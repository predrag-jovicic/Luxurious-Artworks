using AuctionApp.Entities;
using AuctionApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repositories
{
    public class AuctionRepository
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context, IHostingEnvironment hEnvironment)
        {
            _context = context;
            hostingEnvironment = hEnvironment;
        }

        #region Add method
        public Auction Add(string userID, int artworkID, double startingPrice, DateTime dateTime)
        {
            var auctionnew = new Auction
            {
                UserId = userID,
                ArtWorkId = artworkID,
                DateTime = dateTime,
                StartingPrice = startingPrice
            };
            _context.Auctions.Add(auctionnew);
            return auctionnew;
        }
        #endregion

        #region Delete method
        public bool DeleteAuction(int id, string userId)
        {
            var auction = _context.Auctions.FirstOrDefault(au => au.AuctionId == id && au.UserId == userId);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Get methods
        public Auction GetAuction(int auctId)
        {
            return _context.Auctions
                .Include(a => a.ArtWork)
                .Include(a => a.ArtWork.Author)
                .Include(a => a.ArtWork.Category)
                .Include(a => a.User)
                .SingleOrDefault(a => a.AuctionId == auctId);
        }
        public IEnumerable<Auction> GetActiveAuctions()
        {
            return _context.Auctions
                .Include(a => a.ArtWork)
                .Include(a => a.ArtWork.Author)
                .Include(a => a.User)
                .Where(a => a.ArtWork.Sold == false)
                .OrderByDescending(a => a.DateTime).ToList();
        } 
        public IEnumerable<Auction> GetMyAuctions(string userId)
        {
            return _context.Auctions
                .Include(a => a.ArtWork)
                .Include(a => a.ArtWork.Author)
                .Include(a => a.User)
                .Where(a => a.User.Id == userId)
                .OrderByDescending(a => a.DateTime).ToList();
        }
        public double GetStartingPrice(int auctionId)
        {
            return _context.Auctions.Where(auction => auction.AuctionId == auctionId).Select(auction => auction.StartingPrice).Single();
        }
        public bool AtLeastOne(int auctId, string userId)
        {
            return _context.Offers.Where(offer => offer.AuctionId == auctId && offer.Auction.UserId == userId).Any();
        }
        #endregion

        #region Update method
        public bool EndAuction(int auctId, string userId)
        {
            if (AtLeastOne(auctId, userId))
            {
                Auction auction = GetAuction(auctId);
                auction.ArtWork.Sold = true;
                _context.Auctions.Update(auction);
                return true;
            }
            else
                return false;
        } 
        #endregion

        public Auction GetTheHottestAuction()
        {
            return this._context.Auctions.FromSql("SELECT * FROM Auctions WHERE AuctionId = (SELECT TOP(1) a.AuctionId FROM Offers o INNER JOIN Auctions a ON o.AuctionId = a.AuctionId INNER JOIN ArtWorks ar ON ar.ArtWorkId = a.ArtWorkId WHERE Sold = 0 GROUP BY a.AuctionId ORDER BY COUNT(*) DESC)")
                .Include(a => a.ArtWork)
                .Include(a => a.ArtWork.Author)
                .Include(a => a.ArtWork.Category)
                .Include(a => a.User)
                .SingleOrDefault();
        }
    }
}
