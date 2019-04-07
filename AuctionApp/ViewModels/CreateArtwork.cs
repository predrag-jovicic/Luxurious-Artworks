using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class CreateArtwork
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
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
