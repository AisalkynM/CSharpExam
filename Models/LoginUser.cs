using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress]
        public string LoginEmail {get; set; }

        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        public string LoginPassword {get; set; }
    }
}