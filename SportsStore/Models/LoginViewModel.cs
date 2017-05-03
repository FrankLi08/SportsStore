using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SportsStore.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public rolenumber rolenum { get; set; }
        public enum rolenumber
        {
            Manager = 1,
            Customer = 2
        }
    }
}