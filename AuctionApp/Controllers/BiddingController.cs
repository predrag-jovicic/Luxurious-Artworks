using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.Hubs;
using AuctionApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AuctionApp.Controllers
{
    [Route("api/Bidding")]
    public class BiddingController : Controller
    {
        private SignInManager<User> _signInManager;
        private UnitOfWork _unitOfWork;
        private readonly IHubContext<AuctionHub> hubContext;

        public BiddingController(UnitOfWork unitOfWork, SignInManager<User> signinManager, IHubContext<AuctionHub> hubContext)
        {
            _signInManager = signinManager;
            _unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Bid([FromBody]BiddingViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = _signInManager.UserManager.GetUserId(User);
                double startingPrice = 0;
                try
                {
                    startingPrice = _unitOfWork.Auctions.GetStartingPrice(model.AuctionId);
                }
                catch (Exception)
                {
                    return BadRequest("Your request failed");
                }
                if (_unitOfWork.Offers.IsThereAnyOffer(model.AuctionId))
                {
                    double highestBid = _unitOfWork.Offers.GetHighestBid(model.AuctionId);
                    if (model.Amount <= highestBid || model.Amount <= startingPrice)
                        return BadRequest("Your request failed");
                    else
                    {
                        var now = DateTime.UtcNow;
                        var newOffer = new Offer
                        {
                            Amount = model.Amount,
                            AuctionId = model.AuctionId,
                            DateTime = now,
                            UserId = id
                        };
                        _unitOfWork.Offers.Add(newOffer);
                        await _unitOfWork.Save();
                        await this.hubContext.Clients.All.SendAsync("NewBid", $"Currently the highest bid: <strong>${model.Amount}</strong>",model.AuctionId);
                        await this.hubContext.Clients.All.SendAsync("AddOffer",new {
                            Amount = model.Amount,
                            Time = now
                        });
                        return Ok("You have made a new bid");
                    }
                }
                else {
                    if(model.Amount > startingPrice)
                    {
                        var now = DateTime.UtcNow;
                        var newOffer = new Offer
                        {
                            Amount = model.Amount,
                            AuctionId = model.AuctionId,
                            DateTime = now,
                            UserId = id
                        };
                        _unitOfWork.Offers.Add(newOffer);
                        await _unitOfWork.Save();
                        await this.hubContext.Clients.All.SendAsync("NewBid", $"Currently the highest bid: <strong>${model.Amount}</strong>", model.AuctionId);
                        await this.hubContext.Clients.All.SendAsync("AddOffer", new
                        {
                            Amount = model.Amount,
                            Time = now
                        });
                        return Ok("You have made a new bid");
                    }
                    else
                       return BadRequest("Your request failed");
                }
            }
            else
            {
                return BadRequest("Your data is invalid");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetLatestOffers(int auctId)
        {
            if (auctId > 0)
            {
                var currentuserId = _signInManager.UserManager.GetUserId(User);
                var latestbids = _unitOfWork.Offers.GetLatestBidsForParticularAuction(auctId,3);
                return Ok(latestbids);
            }
            else
            {
                return BadRequest("Your request data is not valid");
            }
        }
    }
}