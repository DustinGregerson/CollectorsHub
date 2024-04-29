using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CollectorsHub.Models
{
    public class User : IdentityUser
    {


        [NotMapped]
        public IList<string> RoleNames { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }

        public List<Collection> Collection { get; set; }
        

    }
}
