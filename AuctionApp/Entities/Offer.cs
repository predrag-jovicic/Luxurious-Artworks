using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class Offer
    {
        [Required]
        [Key]
        public int OfferId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        [Range(1,Double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
