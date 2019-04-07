using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "First name is not valid")]
        [RegularExpression("^[A-Z][a-z]{2,22}$")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is not valid")]
        [RegularExpression("^[A-Z][a-z]{2,30}$")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string CurrentPassword { get; set; }
        [DisplayName("Biography")]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        [DisplayName("New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail")]
        public string Email { get; set; }
    }
}
