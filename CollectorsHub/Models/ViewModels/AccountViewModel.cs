using Azure.Identity;

namespace CollectorsHub.Models
{
    public class AccountViewModel
    {
        public AccountViewModel() { }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Error { get; set; }

    }
}
