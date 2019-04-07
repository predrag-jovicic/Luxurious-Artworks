using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class Author
    {
        [Required]
        [Key]
        public int AuthorId { get; set; }
        [Required]
        [RegularExpression("[A-Z][a-z]{2,22}")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("[A-Z][a-z]{2,30}")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public ICollection<ArtWork> ArtWorks { get; set; }
    }
}
