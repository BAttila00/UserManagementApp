using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }

        [Required]
        public string SureName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Place of birth")]
        public string PlaceOfBirth { get; set; }

        [Required]
        [Display(Name = "Place of living")]
        public string PlaceOfLiving { get; set; }


    }
}
