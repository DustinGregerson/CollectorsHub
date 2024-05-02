

using System.ComponentModel.DataAnnotations;

namespace CollectorsHub.Models
{

    public class RegisterViewModel
    {
        public RegisterViewModel() { }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RoleName = "user";
        [Required(ErrorMessage = "You Must Enter Your First Name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Your First Name Can Only Contain Letters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "You Must Enter Your Last Name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Your Last Name Can Only Contain Letters")]
        public string LastName { get; set; }

    }
}
