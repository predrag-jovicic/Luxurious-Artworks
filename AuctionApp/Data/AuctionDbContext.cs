using AuctionApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data
{
    public class AuctionDbContext : IdentityDbContext<User>
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<ArtWork> ArtWorks { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Offer> Offers { get; set; }

    }
}
