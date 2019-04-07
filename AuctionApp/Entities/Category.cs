using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class Category
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte CategoryId { get; set; }
        [Required]
        [RegularExpression("[A-Z][a-z]{1,18}")]
        public string Name { get; set; }
        public ICollection<ArtWork> ArtWorks { get; set; }
    }
}
