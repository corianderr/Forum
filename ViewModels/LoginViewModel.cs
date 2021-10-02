using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Username")]
        public string EmailOrUserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

    }
}
