using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repositories
{
    public class CategoryRepository
    {
        private AuctionDbContext _context;

        public CategoryRepository(AuctionDbContext context)
        {
            _context = context;
        }

        //Get all authors
        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        } 
    }
}
