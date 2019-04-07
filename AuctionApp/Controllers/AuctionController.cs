using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using AuctionApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using AuctionApp.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using AuctionApp.Hubs;

namespace AuctionApp.Controllers
{
    public class AuctionController : Controller
    {
        private SignInManager<User> _signInManager;
        private UnitOfWork _unitOfWork;
        private readonly IHubContext<AuctionHub> hubContext;

        public AuctionController(UnitOfWork unitOfWork, SignInManager<User> signInManager, IHubContext<AuctionHub> hubContext) { 
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuction model)
        {
            if (ModelState.IsValid)
            {
                DateTime current = DateTime.UtcNow;
                string id = _signInManager.UserManager.GetUserId(User);
                bool boolRes = true;
                if (model.Artworkidchosen > 0)
                {
                    if (_unitOfWork.Artworks.IsAvailableToBeSold(model.Artworkidchosen))
                    {
                        var newAuction = _unitOfWork.Auctions.Add(id, model.Artworkidchosen, model.StartingPrice, current);
                        await _unitOfWork.Save();
                        int auctionId = newAuction.AuctionId;
                        var createdAuction = this._unitOfWork.Auctions.GetAuction(auctionId);
                        var data = new
                        {
                            AuctionId = auctionId,
                            ArtWorkName = createdAuction.ArtWork.Name,
                            Author = createdAuction.ArtWork.Author.FirstName + " " + createdAuction.ArtWork.Author.LastName,
                            StartingPrice = createdAuction.StartingPrice,
                            Caption = createdAuction.ArtWork.Caption,
                            PostedOn = createdAuction.DateTime,
                            User = createdAuction.User.FirstName + " " + createdAuction.User.LastName,
                            ReadMoreLink = Url.Action("AuctionDetails", "Auction", new { auct = auctionId })
                        };
                        await this.hubContext.Clients.Group("Authenticated Users").SendAsync("AddAuctionAuthenticated",data);
                        await this.hubContext.Clients.Group("Unauthenticated Users").SendAsync("AddAuctionUnAuthenticated", data);
                    }
                    else
                    {
                        boolRes = false;
                    }
                }
                else
                    boolRes = false;
                if (boolRes)
                    return RedirectToAction(nameof(ResultOperation), new { op = "succeded" });
                else
                    return RedirectToAction(nameof(ResultOperation), new { op = "failed" });
            }
            else
            {
                return View();
            }
        }

        public IActionResult ResultOperation(string op)
        {
            ViewBag.Result = op;
            return View();
        }

        public IActionResult ActiveAuctions()
        {
            try
            {
                var model = _unitOfWork.Auctions.GetActiveAuctions();
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        
        [Authorize]
        public IActionResult MyAuctions()
        {
            try
            {
                var userId = _signInManager.UserManager.GetUserId(User);
                var model = _unitOfWork.Auctions.GetMyAuctions(userId);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }

        [Authorize]
        public async Task<IActionResult> DeleteAuction(int del)
        {
            try
            {
                string userId = _signInManager.UserManager.GetUserId(User);
                _unitOfWork.Offers.DeleteOffers(del);
                bool flag = _unitOfWork.Auctions.DeleteAuction(del, userId);
                await _unitOfWork.Save();
                if (flag)
                {
                    await this.hubContext.Clients.All.SendAsync("DeleteAuction", "This auction has been deleted.", del);
                    await this.hubContext.Clients.All.SendAsync("DeleteAuctionDetailsPage", "This auction has been deleted.", Url.Action("ActiveAuctions", "Auction"));
                    return RedirectToAction(nameof(ResultOperation), new { op = "succeded" });
                }
                else
                    return RedirectToAction(nameof(ResultOperation), new { op = "failed" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }


        [Authorize]
        public IActionResult Offers(int auct, int page = 1)
        {
            try
            {
                string userId = _signInManager.UserManager.GetUserId(User);
                ViewBag.NumberOfPages = (int)Math.Ceiling(_unitOfWork.Offers.AuctionOffersCount(userId, auct) / 10.0); 
                ViewBag.AuctionId = auct;
                ViewBag.CurrentPage = page;
                int skipNumber = page > 1 ? (page - 1) * 10 : 0;
                var offers = _unitOfWork.Offers.GetLatestBidsForParticularAuction(auct, 10, skipNumber);
                return View(offers);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        [Authorize]
        public async Task<IActionResult> EndAuction(int auct)
        {
            try
            {
                string userId = _signInManager.UserManager.GetUserId(User);
                bool flag = _unitOfWork.Auctions.EndAuction(auct, userId);
                await _unitOfWork.Save();
                if (flag)
                {
                    await this.hubContext.Clients.All.SendAsync("EndAuction", "This auction has ended.", auct);
                    await this.hubContext.Clients.All.SendAsync("EndAuctionDetailsPage", "This auction has ended.", Url.Action("ActiveAuctions", "Auction"));
                    return RedirectToAction(nameof(ResultOperation), new { op = "succeded" });
                }
                else
                    return RedirectToAction(nameof(ResultOperation), new { op = "failed. The auction hasn't got any bids yet. Therefore, it can't be terminated." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        [Authorize]
        public IActionResult MyBids(int page = 1)
        {
            try
            {
                var userId = _signInManager.UserManager.GetUserId(User);
                ViewBag.NumberOfPages = (int)Math.Ceiling(_unitOfWork.Offers.MyBidsCount(userId) / 10.0);
                ViewBag.CurrentPage = page;
                int skipNumber = page > 1 ? (page - 1) * 10 : 0;
                var bids = _unitOfWork.Offers.GetMyOffers(userId,skipNumber).Take(10);
                return View(bids);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
        public IActionResult AuctionDetails(int auct)
        {
            try
            {
                var auction = _unitOfWork.Auctions.GetAuction(auct);
                var model = new AuctionDetails
                {
                    AuctionId = auction.AuctionId,
                    Author = auction.ArtWork.Author.FirstName + " " + auction.ArtWork.Author.LastName,
                    Caption = auction.ArtWork.Caption,
                    DateTime = auction.DateTime,
                    Category = auction.ArtWork.Category.Name,
                    Image = auction.ArtWork.Image,
                    Name = auction.ArtWork.Name,
                    Sold = auction.ArtWork.Sold,
                    StartingPrice = auction.StartingPrice,
                    UserPosted = auction.User.UserName
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ResultOperation), new { op = "failed. Internal Server Error." });
            }
        }
    }
}
