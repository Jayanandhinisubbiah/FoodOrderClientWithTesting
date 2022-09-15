using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallingAPIInClient.Models
{
    public class UserList
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        [Required(ErrorMessage = "*")]
        public string FName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Lname { get; set; }
        public string Gender { get; set; }

        //[EmailAddress]
        [Required(ErrorMessage = "Please enter an valid Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email ID")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? CPassword { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
