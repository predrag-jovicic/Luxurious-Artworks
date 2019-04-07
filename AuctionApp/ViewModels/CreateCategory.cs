using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class CreateCategory
    {
        [Required(ErrorMessage = "Category name is not valid")]
        [RegularExpression("^[A-z\\s]{1,18}$")]
        [DisplayName("Category name")]
        public string CategoryName { get; set; }
    }
}
