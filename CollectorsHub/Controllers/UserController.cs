using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollectorsHub.Controllers
{
    public class UserController : Controller
    {
        CollectorsHubUnitOfWork data;
        public UserController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult List(string filterUserName,string filterCollectionTag)
        {

            UserViewModel model = new UserViewModel();
            model.users = data.Users.List(new QueryOptions<User>
            {
                Include = "Collection",
                Where = (user => user.UserName != "Admin"),
            }).ToList();

            model.UserNameFilterString = filterUserName;
            model.CollectionTagFilterString = filterCollectionTag;
            model.GetUsers();

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
