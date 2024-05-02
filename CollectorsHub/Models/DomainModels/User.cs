using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;

namespace CollectorsHub.Models
{
    public class User : IdentityUser
    {


        [NotMapped]
        public IList<string>? RoleNames { get; set; }
        
        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        public List<Collection>? Collection { get; set; }
        

    }
}
