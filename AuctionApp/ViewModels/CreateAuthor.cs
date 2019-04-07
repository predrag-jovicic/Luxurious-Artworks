using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class CreateAuthor
    {
        [Required(ErrorMessage = "First name is not valid")]
        [RegularExpression("[A-Za-z\\s]{2,22}")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is not valid")]
        [RegularExpression("[A-Za-z\\s]{2,30}")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
