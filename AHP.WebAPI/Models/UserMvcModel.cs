using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class UserMvcModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "You need a username")]
        public string Username { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "You need a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}