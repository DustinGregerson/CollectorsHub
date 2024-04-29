

namespace CollectorsHub.Models
{

    public class RegisterViewModel
    {
        public RegisterViewModel() { }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RoleName = "user";
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
