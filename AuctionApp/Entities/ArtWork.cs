using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class ArtWork
    {
        [Required]
        [Key]
        public int ArtWorkId { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Caption { get; set; }
        [Required]
        public bool Sold { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public byte CategoryId { get; set; }
        [Required]
        public string Image { get; set; }
        public Author Author { get; set; }
        public Category Category { get; set; }
        public Auction Auction { get; set; }
    }
}
