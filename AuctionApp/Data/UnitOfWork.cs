using AuctionApp.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data
{
    public class UnitOfWork
    {
        public ArtworksRepository Artworks { get; private set; }
        public AuctionRepository Auctions { get; private set; }
        public AuthorsRepository Authors { get; private set; }
        public CategoryRepository Categories { get; private set; }
        public UserRepository Users { get; private set; }
        public OffersRepository Offers { get; private set; }

        private IHostingEnvironment _hostingEnvironment;
        public AuctionDbContext _context;

        public UnitOfWork(AuctionDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            Artworks = new ArtworksRepository(context);
            Auctions = new AuctionRepository(context,hostingEnvironment);
            Authors = new AuthorsRepository(context);
            Categories = new CategoryRepository(context);
            Users = new UserRepository(context,hostingEnvironment);
            Offers = new OffersRepository(context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
