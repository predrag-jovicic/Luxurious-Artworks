using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuctionApp.Data
{
    public class Seeder
    {
        private AuctionDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public Seeder(AuctionDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Seed()
        {
            await CreateUsers();
            CreateCategories();
            CreateAuthors();
            CreateArtWorks();
        }

        private void CreateCategories()
        {
            if (!_context.Categories.Any())
            {
                var category = new Category
                {
                    Name = "Paintings"
                };
                _context.Categories.Add(category);
                category = new Category
                {
                    Name = "Drawings"
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
        }

        private async Task CreateUsers()
        {
            if (!_userManager.Users.Any())
            { 
                var role = new IdentityRole("admins");
                await _roleManager.CreateAsync(role);
                role = new IdentityRole("authusers");
                await _roleManager.CreateAsync(role);
                var user = new User
                {
                    FirstName = "Predrag",
                    LastName = "Jovicic",
                    UserName = "adminpeksi",
                    Email = "predragjovicic33@hotmail.com"
                };
                await _userManager.UpdateSecurityStampAsync(user);
                var result = await _userManager.CreateAsync(user, "P@ssWord3!");
                if (!result.Succeeded)
                    throw new InvalidOperationException();
                result = await _userManager.AddToRoleAsync(user, "admins");
                if (!result.Succeeded)
                    throw new InvalidOperationException();
            }
        }

        private void CreateArtWorks()
        {
            if (!_context.Authors.Any())
            {
                var artwork = new ArtWork
                {
                    Name = "Mona Lisa",
                    Caption = "The painting is thought to be a portrait of Lisa Gherardini, the wife of Francesco del Giocondo, and is in oil on a white Lombardy poplar panel. It had been believed to have been painted between 1503 and 1506; however, Leonardo may have continued working on it as late as 1517. Recent academic work suggests that it would not have been started before 1513.",
                    AuthorId = 1,
                    CategoryId = 1
                };
                _context.ArtWorks.Add(artwork);
                artwork = new ArtWork
                {
                    Name = "The Last Supper",
                    Caption = "The painting represents the scene of the Last Supper of Jesus with his apostles, as it is told in the Gospel of John, 13:21. Leonardo has depicted the consternation that occurred among the Twelve Disciples when Jesus announced that one of them would betray him.",
                    AuthorId = 1,
                    CategoryId = 1
                };
                _context.ArtWorks.Add(artwork);
                artwork = new ArtWork
                {
                    Name = "Vitruvian Man",
                    Caption = "The drawing is based on the correlations of ideal human proportions with geometry described by the ancient Roman architect Vitruvius in Book III of his treatise De architectura. Vitruvius described the human figure as being the principal source of proportion among the classical orders of architecture. Vitruvius determined that the ideal body should be eight heads high. Leonardo's drawing is traditionally named in honor of the architect.",
                    AuthorId = 1,
                    CategoryId = 2
                };
                _context.ArtWorks.Add(artwork);
                _context.SaveChanges();
            }
        }

        private void CreateAuthors()
        {
            _context.Database.EnsureCreated();
            if (!_context.Authors.Any())
            {
                var author = new Author
                {
                    FirstName = "Leonardo",
                    LastName = "Da Vinci",
                };
                author = new Author
                {
                    FirstName = "Vincent",
                    LastName = "Van Gogh",
                };
                _context.Authors.Add(author);
                author = new Author
                {
                    FirstName = "Pablo",
                    LastName = "Picasso",
                };
                _context.Authors.Add(author);
                author = new Author
                {
                    FirstName = "Claude",
                    LastName = "Monet",
                };
                _context.Authors.Add(author);
                _context.SaveChanges();
            }
        }
    }
}
