using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private UnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(SignInManager<User> signInManager, UserManager<User> userManager, UnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            Author author = this._unitOfWork.Authors.TheTopSellingArtist();
            Auction auction = this._unitOfWork.Auctions.GetTheHottestAuction();
            TrendingViewModel vm = new TrendingViewModel
            {
                Author = author,
                Auction = auction
            };
            return View(vm);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }
        public IActionResult ResultOperation(string op)
        {
            ViewBag.Result = op;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
                if (!result.Succeeded)
                    ModelState.AddModelError("", "Wrong credentials");
                else
                {
                    string username = loginViewModel.Username;
                    User currentUser = _userManager.Users.Single(u => u.UserName == username);
                    if(await _userManager.IsInRoleAsync(currentUser,"admins"))
                        return RedirectToAction("Index", "Artworks");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _signInManager.SignOutAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            else
                return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username,
                        Email = model.Email
                    };
                    await _userManager.UpdateSecurityStampAsync(user);
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, "authusers");
                        if (result.Succeeded)
                            return RedirectToAction("Succeded", "Auction");
                        else
                            ModelState.AddModelError("", "Error ocurred while trying to register");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Check your password and username. Password must have at least one uppercased character, one number and one special character. Username must be unique.");
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        [Authorize]
        public IActionResult MyProfile()
        {
            try
            {
                string userId = _userManager.GetUserId(User);
                User user = _unitOfWork.Users.GetUser(userId);
                var myAuctions = _unitOfWork.Auctions.GetMyAuctions(userId).Take(3);
                var myBids = _unitOfWork.Offers.GetMyOffers(userId).Take(3);
                var model = new UserProfileViewModel
                {
                    User = user,
                    MyAuctions = myAuctions,
                    MyBids = myBids
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProfile()
        {
            try
            {
                var user = _unitOfWork.Users.GetUser(_signInManager.UserManager.GetUserId(User));
                var model = new EditProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Bio = user.Biography
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _unitOfWork.Users.GetUser(_signInManager.UserManager.GetUserId(User));
                if (model.NewPassword != null && model.CurrentPassword != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                        if (!result.Succeeded)
                            return RedirectToAction(nameof(ResultOperation), new { op = "failed. Password couldn't be changed" });
                    }
                    else
                        return RedirectToAction(nameof(ResultOperation), new { op = "failed. Your old password is not correct" });
                }
                else return View(model);
                if (model.Image != null)
                {
                    string type = model.Image.ContentType.Split("/")[0];
                    if (type != "image")
                        throw new InvalidDataException();
                    string currentFileName = model.Image.FileName.Trim('"');
                    string fileExtension = Path.GetExtension(currentFileName);
                    string newFileName = Guid.NewGuid().ToString() + fileExtension;
                    string semiPath = $@"images\profileimages\{newFileName}";
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, semiPath);
                    string dbPath = $"/images/profileimages/{newFileName}";
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(stream);
                        stream.Flush();
                    }
                    if (user.Image != null)
                    {
                        string delPath = user.Image.Replace("/", @"\");
                        string fulldelPath = _hostingEnvironment.WebRootPath + delPath;
                        System.IO.File.Delete(fulldelPath);
                    }
                    user.Image = dbPath;
                }
                else
                {
                    return RedirectToAction(nameof(ResultOperation), new { op = "failed. The file you uploaded is not in a correct format" });
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Biography = model.Bio;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(ResultOperation), new { op = "succeded." });
            }
            else
                return View(model);
        }
    }
}