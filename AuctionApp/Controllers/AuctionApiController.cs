using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionApp.Controllers
{
    [Route("api/[controller]")]
    public class AuctionApiController : Controller
    {
        private UnitOfWork _unitOfWork;
        private UserManager<User> _uManager;
        private readonly IHubContext<AuctionHub> hubContext;

        public AuctionApiController(UnitOfWork unitOfWork, UserManager<User> userManager, IHubContext<AuctionHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _uManager = userManager;
            this.hubContext = hubContext;
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> EndAuction(int auctId)
        {
            var userId = _uManager.GetUserId(User);
            bool resultBool = _unitOfWork.Auctions.EndAuction(auctId, userId);
            await _unitOfWork.Save();
            if (resultBool)
            {
                await this.hubContext.Clients.All.SendAsync("EndAuction", "This auction has ended.",auctId);
                await this.hubContext.Clients.All.SendAsync("EndAuctionDetailsPage", "This auction has ended.",Url.Action("ActiveAuctions", "Auction"));
                return NoContent();
            }
            else
                return BadRequest("The auction hasn't got any bids yet. Therefore, it can't be terminated.");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAuction(int auctId)
        {
            var userId = _uManager.GetUserId(User);
            _unitOfWork.Offers.DeleteOffers(auctId);
            bool resultBool = _unitOfWork.Auctions.DeleteAuction(auctId, userId);
            await _unitOfWork.Save();
            if (resultBool)
            {
                await this.hubContext.Clients.All.SendAsync("DeleteAuction", "This auction has been deleted.",auctId);
                await this.hubContext.Clients.All.SendAsync("DeleteAuctionDetailsPage", "This auction has been deleted.", Url.Action("ActiveAuctions", "Auction"));
                return NoContent();
            }
            else
                return BadRequest("The auction is not found");
        }
    }
}
