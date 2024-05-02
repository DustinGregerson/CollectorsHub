using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CollectorsHub.Models
{
    public class UserViewModel
    {
        public UserViewModel() { 
        
        }
        public string UserNameFilterString { get; set; }
        public string CollectionTagFilterString { get; set; }
        public List<User> users { get; set; }

        public List<string> CollectionTags { get; set; }
    }
}
