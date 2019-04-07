using AuctionApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class CreateAuction
    {
        [Required]
        [Range(1,int.MaxValue,ErrorMessage = "You didn't choose an artwork")]
        public int Artworkidchosen { get; set; }
        [Required]
        [Range(1, Double.MaxValue,ErrorMessage = "Starting price must be greater than 0")]
        [DisplayName("Starting price")]
        public double StartingPrice { get; set; }
    }
}
