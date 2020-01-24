using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Exam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MinLength(3, ErrorMessage="First Name must be at least 3 characters")]
        [Display(Name="First Name")]
        public string FirstName {get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(3, ErrorMessage="Last Name must be at least 3 characters")]
        [Display(Name="Last Name")]
        public string LastName {get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email {get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password doesn't match.")]
        [MyPassword]
        public string Password {get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [MyPassword]
        public string ConfirmPassword {get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; }= DateTime.Now;

        //NAVIGATIONAL PROPERTY here()
        // -----------------Many To Many ()----------------------------
        public List <Participation> GoingTo {get; set;}

        // -----------------One To Many ()----------------------------
        public List <Banana> PlannedBananas {get; set;}

    }
    public class MyPasswordAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = (string)value;

            if(string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult("Password should not be empty.");
            }
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{4,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if(!hasLowerChar.IsMatch(input))
            {
                return new ValidationResult("Password should contain at least one lower case letter.");
            }
            else if(!hasUpperChar.IsMatch(input))
            {
                return new ValidationResult("Password should contain At least one upper case letter");
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                return new ValidationResult("Password should not be less than or greater than 8 characters");
            }
            else if (!hasNumber.IsMatch(input))
            {
                return new ValidationResult("Password should contain At least one numeric value");
            }
            else if (!hasSymbols.IsMatch(input))
            {
                return new ValidationResult("Password should contain At least one special case characters");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}