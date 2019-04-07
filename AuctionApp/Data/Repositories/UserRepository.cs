using AuctionApp.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repositories
{
    public class UserRepository
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private AuctionDbContext _context;

        public UserRepository(AuctionDbContext context, IHostingEnvironment hEnvironment)
        {
            _context = context;
            hostingEnvironment = hEnvironment;
        }

        #region Get methods
        public User GetUser(string userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }
        #endregion

        #region Update method
        public void Update(User user)
        {
            _context.Users.Update(user);
        } 
        #endregion
    }
}
