using CollectorsHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CollectorsHub.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CollectionController : Controller
    {
        CollectorsHubUnitOfWork data;
        public CollectionController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);


        public IActionResult ListUserCollections(string Id)
        {
            HttpContext.Session.SetString("CurrentUser", Id);


            List<Collection> collection = data.Collections.List(new QueryOptions<Collection>
            {
                Include = "Items",
                Where = (col => col.UserId == Id)
            }).ToList();

            return View(collection);
        }
        public IActionResult Delete(int CollectionId)
        {
            
            data.Collections.Delete(data.Collections.Get(CollectionId));
            data.Save();
            return RedirectToAction("ListUserCollections",new { Id =HttpContext.Session.GetString("CurrentUser")  });
        }

    }
}
