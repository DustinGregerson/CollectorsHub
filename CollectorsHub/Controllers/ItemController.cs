using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollectorsHub.Controllers
{

    public class ItemController : Controller
    {
        CollectorsHubUnitOfWork data;
        public ItemController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        [HttpGet]
        public IActionResult AddEdit(int id)
        {
            if(id == 0)
            {
                ItemViewModel model = new ItemViewModel();
                return View(model);
            }
            else
            {
                ItemViewModel model = new ItemViewModel();
                model.Edit = true;
                model.item = data.Items.Get(id);
                model.ImagePNGbase64 = ImageConverter.byteArrayTo64BaseEncode(model.item.image);
                return View(model);
            }

        }
        [HttpPost]
        public IActionResult AddEdit(ItemViewModel model)
        {
            return View();
        }
    }
}
