using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Banana
    {
        [Key]
        public int BananaId {get; set;}

        [Required(ErrorMessage="Title is required")]
        [Display(Name="Title : ")]
        public string Title {get; set;}

        [Required(ErrorMessage="Date is required")]
        [FutureDate]
        public DateTime Date {get; set;}

        [Required(ErrorMessage="Duration is required")]
        [Display(Name="Duration : ")]
        public int Duration {get; set;}

        [Required(ErrorMessage="Duration Type is required")]
        [Display(Name="-")]
        public string DurationType {get; set;}

        [Required(ErrorMessage="Description is required")]
        [Display(Name="Description : ")]
        public string Description {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        // ---------OneToMany ( a wediding can only have one planner) ----------
        public int UserId {get; set;}
        public User Planner {get; set;}
        
        // ---------ManyToMant ( a wediding can have many guests) ----------
        public List<Participation> GuestList {get; set;}


    }
    public class FutureDateAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime check;
            if(value is DateTime)
            {
                check = (DateTime)value;
                if(check < DateTime.Now)
                {
                    return new ValidationResult("Date has to be in the future.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Enter a real date.");
            }
        }
    }
}