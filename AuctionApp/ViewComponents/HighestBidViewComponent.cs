using AuctionApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewComponents
{
    public class HighestBidViewComponent : ViewComponent
    {
        private UnitOfWork _unitOfWork;

        public HighestBidViewComponent(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke(int auct)
        {
            string amount = _unitOfWork.Offers.GetHighestBid(auct).ToString();
            return View("HighestBid", amount);
        }
    }
}
