using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewComponents
{
    public class LatestBidsViewComponent : ViewComponent
    {
        private SignInManager<User> _signInManager;
        private UnitOfWork _unitOfWork;

        public LatestBidsViewComponent(UnitOfWork unitOfWork, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int auct)
        {
            var currentuserId = _signInManager.UserManager.GetUserId(UserClaimsPrincipal);
            var latbids = _unitOfWork.Offers.GetLatestBidsForParticularAuction(auct, 3);
            var auction = _unitOfWork.Auctions.GetAuction(auct);
            var viewmodel = new LatestOffersViewModel
            {
                LatestBids = latbids,
                AuctionId = auct,
                CurrentUser = currentuserId,
                UserCreated = auction.UserId
            };
            return View("LatestBids",viewmodel);
        }
    }
}
