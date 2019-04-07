using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [RegularExpression("^[A-Z][a-z]{2,22}$")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[A-Z][a-z]{2,30}$")]
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Biography { get; set; }
    }
}
