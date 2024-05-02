using CollectorsHub.Models;
using CollectorsHub.Models.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CollectorsHub.Controllers
{
    public class UserController : Controller
    {
        CollectorsHubUnitOfWork data;
        public UserController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult List(string filterUserName, string filterCollectionTag)
        {
            
            UserViewModel model = new UserViewModel();
            //filter
            if (filterCollectionTag != "All" && filterCollectionTag != null
             && filterUserName != "All" && filterUserName != null)
            {
                model.users = data.Users.List(new QueryOptions<User>
                {
                    Include = "Collection",
                    Where = (user => user.UserName != "Admin" && user.UserName.Contains(filterUserName))
                }).Select(user =>
                {
                    user.Collection = user.Collection.Where(col => col.Tag == filterCollectionTag).ToList();
                    return user;

                }).ToList();
            }
            else if (filterCollectionTag == "All" && filterUserName != "All")
            {
                model.users = data.Users.List(new QueryOptions<User>
                {
                    Include = "Collection",
                    Where = (user => user.UserName != "Admin" && user.UserName.Contains(filterUserName))
                }).ToList();
            }
            else if (filterCollectionTag != "All" && filterUserName == "All")
            {
                model.users = data.Users.List(new QueryOptions<User>
                {
                    Include = "Collection",
                    Where = (user => user.UserName != "Admin")
                }).Select(user =>
                {
                    user.Collection = user.Collection.Where(col => col.Tag == filterCollectionTag).ToList();
                    return user;

                }).Where(user=>user.Collection.Count()>0).ToList();
            }
            else
            {
                model.users = data.Users.List(new QueryOptions<User>
                {
                    Include = "Collection",
                    Where = (user => user.UserName != "Admin")
                }).ToList();
            }

            List<Collection> collections = UserViewModelExtension.ExtractCollections<User, Collection>(model.users, user => user.Collection);
            model.CollectionTags = UserViewModelExtension.GetDistinctTags<Collection>(collections, col =>col.Tag);

            return View(model);
        }
        public IActionResult filter(string UserNameFilterString, string filterCollectionTag)
        {
            if (UserNameFilterString == null)
            {
                UserNameFilterString = "All";
            }
            string customRoute = "List/filter/" + UserNameFilterString + "/" + filterCollectionTag;
            return Redirect(customRoute);
        }
    }
}
