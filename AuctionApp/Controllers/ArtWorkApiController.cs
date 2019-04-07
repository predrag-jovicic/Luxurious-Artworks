using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers
{
    [Route("api/SearchArtWork")]
    public class ArtWorkApiController : Controller
    {
        private UnitOfWork _unitOfWork;

        public ArtWorkApiController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public IActionResult SearchForArtWork(string partialText)
        {
            return Ok(_unitOfWork.Artworks.GetArtWorks(partialText));
        }
    }
}