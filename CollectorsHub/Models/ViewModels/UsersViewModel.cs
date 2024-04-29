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
        private Regex regex;
        public List<User> users { get; set; }

        public List<String> CollectionTags { get; set; }

        public List<User> GetUsers() {
            CollectionTags = new List<string>();
            if(CollectionTags.Count==0)
            {
                foreach(User user in users)
                {
                    foreach(Collection collection in user.Collection)
                    {
                        if (!CollectionTags.Contains(collection.Tag))
                        {
                            CollectionTags.Add(collection.Tag);
                        }
                    }
                } 
            }

            if(CollectionTagFilterString != "All" && CollectionTagFilterString !=null)
            {
                users = users.Where(user=>user.Collection.Where(col=>col.Tag==CollectionTagFilterString).Any()).ToList();
            }

            if (UserNameFilterString == null||UserNameFilterString=="All")
            {
                return users;
            }
            else
            {
                regex = new Regex(".*" + UserNameFilterString + ".*");
                users = users.Where(user => regex.IsMatch(user.UserName)).ToList();
                return users;
            }


        }
    }
}
