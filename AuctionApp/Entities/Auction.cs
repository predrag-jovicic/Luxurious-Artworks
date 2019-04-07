using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class Auction
    {
        [Required]
        [Key]
        public int AuctionId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        [Range(0,Double.MaxValue)]
        public double StartingPrice { get; set; }
        [Required]
        public int ArtWorkId { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ArtWork ArtWork { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
