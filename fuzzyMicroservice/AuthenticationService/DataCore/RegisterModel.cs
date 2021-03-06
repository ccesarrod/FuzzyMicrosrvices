﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.DataCore
{
    public class RegisterModel
    {
        [Required]
        //   [StringLength(5, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
