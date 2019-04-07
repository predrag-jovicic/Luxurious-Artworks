using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required
        (ErrorMessage = "First name is not valid")]
        [DisplayName("First name")]
        [RegularExpression("^[A-Z][a-z]{2,22}$")]
        public string FirstName { get; set; }
        [Required
        (ErrorMessage = "Last name is not valid")]
        [RegularExpression("^[A-Z][a-z]{2,30}$")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required
        (ErrorMessage = "Username format is not valid")]
        [RegularExpression("^[A-z0-9]+$")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
