using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class BiddingViewModel
    {
        [Required]
        [Range(1, Double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public int AuctionId { get; set; }
    }
}
