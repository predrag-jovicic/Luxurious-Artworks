using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class LoginViewModel
    {
        [Required
        (ErrorMessage = "Username format is not valid")]
        [RegularExpression("^[A-z0-9]+$")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
