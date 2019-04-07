using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AuctionApp.Controllers
{
    public class ArtWorksController : Controller
    {
        private readonly AuctionDbContext _context;
        private IHostingEnvironment hostingEnvironment;

        public ArtWorksController(AuctionDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: ArtWorks
        public async Task<IActionResult> Index()
        {
            var auctionDbContext = _context.ArtWorks.Include(a => a.Author).Include(a => a.Category);
            return View(await auctionDbContext.ToListAsync());
        }

        // GET: ArtWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = await _context.ArtWorks
                .Include(a => a.Author)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(m => m.ArtWorkId == id);
            if (artWork == null)
            {
                return NotFound();
            }

            return View(artWork);
        }

        // GET: ArtWorks/Create
        public IActionResult Create()
        {
            ViewData["Author"] = _context.Authors.Select(a => new SelectListItem
            {
                Text = $"{a.FirstName} {a.LastName}",
                Value = a.AuthorId.ToString()
            });
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: ArtWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArtwork artWork)
        {
            if (ModelState.IsValid)
            {
                string type = artWork.Image.ContentType.Split("/")[0];
                if (type != "image")
                    throw new InvalidDataException();
                string currentFileName = artWork.Image.FileName.Trim('"');
                string fileExtension = Path.GetExtension(currentFileName);
                string newFileName = Guid.NewGuid().ToString() + fileExtension;
                string semiPath = $@"images\artworkimages\{newFileName}";
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, semiPath);
                string dbPath = $"/images/artworkimages/{newFileName}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    artWork.Image.CopyTo(stream);
                    stream.Flush();
                }
                var newArtwork = new ArtWork
                {
                    Name = artWork.Name,
                    CategoryId = artWork.CategoryId,
                    AuthorId = artWork.AuthorId,
                    Caption = artWork.Caption,
                    Sold = false,
                    Image = dbPath
                };
                _context.Add(newArtwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Author"] = _context.Authors.Select(a => new SelectListItem
            {
                Text = $"{a.FirstName} {a.LastName}",
                Value = a.AuthorId.ToString()
            });
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(artWork);
        }

        // GET: ArtWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = await _context.ArtWorks.SingleOrDefaultAsync(m => m.ArtWorkId == id);
            if (artWork == null)
            {
                return NotFound();
            }
            ViewData["Author"] = _context.Authors.Select(a => new SelectListItem
            {
                Text = $"{a.FirstName} {a.LastName}",
                Value = a.AuthorId.ToString()
            });
            ViewData["Category"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString()
            });
            CreateArtwork vm = new CreateArtwork
            {
                ArtWorkId = artWork.ArtWorkId,
                Caption = artWork.Caption,
                Sold = artWork.Sold,
                Name = artWork.Name,
                AuthorId = artWork.AuthorId,
                CategoryId = artWork.CategoryId
            };
            return View(vm);
        }

        // POST: ArtWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateArtwork artWork)
        {
            var found = this._context.ArtWorks.Find(artWork.ArtWorkId);
            if (found == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(artWork.Image != null)
                    {
                        string type = artWork.Image.ContentType.Split("/")[0];
                        if (type != "image")
                            throw new InvalidDataException();
                        string currentFileName = artWork.Image.FileName.Trim('"');
                        string fileExtension = Path.GetExtension(currentFileName);
                        string newFileName = Guid.NewGuid().ToString() + fileExtension;
                        string semiPath = $@"images\artworkimages\{newFileName}";
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, semiPath);
                        string dbPath = $"/images/artworkimages/{newFileName}";
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            artWork.Image.CopyTo(stream);
                            stream.Flush();
                        }
                        found.Image = dbPath;
                    }
                    found.Name = artWork.Name;
                    found.Caption = artWork.Caption;
                    found.Sold = artWork.Sold;
                    found.CategoryId = artWork.CategoryId;
                    found.AuthorId = artWork.AuthorId;
                    _context.ArtWorks.Update(found);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtWorkExists(artWork.ArtWorkId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Author"] = _context.Authors.Select(a => new SelectListItem
            {
                Text = $"{a.FirstName} {a.LastName}",
                Value = a.AuthorId.ToString(),
                Selected = (a.AuthorId == artWork.AuthorId)
            });
            ViewData["Category"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString(),
                Selected = (c.CategoryId == artWork.CategoryId)
            });
            return View(artWork);
        }

        // GET: ArtWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = await _context.ArtWorks
                .Include(a => a.Author)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(m => m.ArtWorkId == id);
            if (artWork == null)
            {
                return NotFound();
            }

            return View(artWork);
        }

        // POST: ArtWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artWork = await _context.ArtWorks.SingleOrDefaultAsync(m => m.ArtWorkId == id);
            _context.ArtWorks.Remove(artWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtWorkExists(int id)
        {
            return _context.ArtWorks.Any(e => e.ArtWorkId == id);
        }
    }
}
