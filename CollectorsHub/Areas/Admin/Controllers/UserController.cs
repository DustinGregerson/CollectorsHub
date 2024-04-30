using CollectorsHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CollectorsHub.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        CollectorsHubUnitOfWork data;
        public UserController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult ListUsers()
        {
            List<User> users = data.Users.List(new QueryOptions<User> { Where = (user=>user.UserName!="Admin") }).ToList();
            return View(users);
        }

        public IActionResult DeleteUser(string id)
        {
            data.Users.Delete(data.Users.Get(id));
            data.Save();
            return RedirectToAction("ListUsers");
        }
    }
}
