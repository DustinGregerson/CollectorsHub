using CollectorsHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CollectorsHub.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ItemController : Controller
    {
        CollectorsHubUnitOfWork data;
        public ItemController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult Delete(int ItemId)
        {
            data.Items.Delete(data.Items.Get(ItemId));
            data.Save();
            return Redirect("/Admin/Collection/ListUserCollections/"+HttpContext.Session.GetString("CurrentUser"));
        }
    }
}
