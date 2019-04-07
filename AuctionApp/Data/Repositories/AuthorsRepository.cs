using AuctionApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data
{
    public class AuthorsRepository
    {
        private AuctionDbContext _context;

        public AuthorsRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public Author TheTopSellingArtist()
        {
            return this._context.Authors.FromSql("SELECT * FROM Authors WHERE AuthorId = (SELECT TOP(1) a.AuthorId FROM ArtWorks a INNER JOIN Authors au ON a.AuthorId = au.AuthorId WHERE Sold = 1 GROUP BY a.AuthorId ORDER BY COUNT(*) DESC)").SingleOrDefault();
        }

        // Get all authors
        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        } 
    }
}
