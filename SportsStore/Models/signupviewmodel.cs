using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportStore.Domain.Entities;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class signupviewmodel:Attribute
    {   
        [Required]
        [StringLength(50)]
        public string user_name { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      
        public string password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("password")]
        public string comfirmedpasswor { get; set; }
        public rolenumber rolenum { get; set; }
        public enum rolenumber
        {
            Manager=1,
            Customer=2
        }




    }
}