using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repositories
{
    public class ArtworksRepository
    {
        private AuctionDbContext _context;

        public ArtworksRepository(AuctionDbContext context)
        {
            _context = context;
        }

        #region Get methods
        public IEnumerable<ArtWork> GetArtWorks(string partialText)
        {
            var query = _context.ArtWorks
                .Where(a => a.Sold == false && a.Name.ToUpper().StartsWith(partialText) && a.ArtWorkId != a.Auction.ArtWorkId);
            return query.ToList();
        }
        public IEnumerable<ArtWork> GetArtWorks()
        {
            return _context.ArtWorks.ToList();
        }
        public bool IsAvailableToBeSold(int artworkId)
        {
            var isSold = _context.ArtWorks.Any(a => a.Sold == true && a.ArtWorkId == artworkId);
            var isBeingOffered = _context.Auctions.Any(a => a.ArtWorkId == artworkId);
            if (!isSold && !isBeingOffered)
                return true;
            else
                return false;
        }
        #endregion

    }
}
