using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CollectorsHub.Controllers
{
    public class HomeController : Controller
    {
        private CollectorsHubUnitOfWork data { get; set; }
        public HomeController(CollectorsHubContext ctx) =>data = new CollectorsHubUnitOfWork(ctx);
        public IActionResult Index(string id="value")
        {
            return View("Index",id);
        }

    }
}