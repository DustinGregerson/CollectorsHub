using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CollectorsHub.Controllers
{

    public class ItemController : Controller
    {
        CollectorsHubUnitOfWork data;
        public ItemController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        [HttpGet]
        public IActionResult AddEdit(int ItemId,int CollectionId)
        {
            if(ItemId == 0)
            {
                TempData["action"] = "Add";
                TempData["CollectionId"] = CollectionId;
                TempData["ItemId"] = ItemId;
                ItemViewModel model = new ItemViewModel();
                return View(model);
            }
            else
            {
                TempData["action"] = "Edit";
                TempData["CollectionId"]=CollectionId;
                TempData["ItemId"] = ItemId;
                ItemViewModel model = new ItemViewModel();
                model.Edit = true;
                model.item = data.Items.Get(ItemId);
                model.ImagePNGbase64 = ImageConverter.byteArrayTo64BaseEncode(model.item.image);
                return View(model);
            }

        }
        [HttpPost]
        public IActionResult AddEdit(ItemViewModel model)

        {
            //setting the collection and then user to collection to create a valid state
            int CollectionId = int.Parse(TempData["CollectionId"].ToString());
            model.item.CollectionId = CollectionId;
            model.item.Collection = data.Collections.Get(CollectionId);
            model.item.Collection.User = data.Users.Get(new QueryOptions<User>
            {
                Where = (user => user.UserName == User.Identity.Name)
            });


            //This section checks to see if a img file was passed to the model.
            //if not a attempt is made to see if the model contains a base 64 image
            //if i doesn't contain one then the model state will be invalid
            int ItemId = int.Parse(TempData["ItemId"].ToString());

            if (model.imgFile != null)
            {
                model.item.image = ImageConverter.iFormImageFileToByteArray(model.imgFile);
            }
            else if(model.imgFile==null && model.ImagePNGbase64!=null){
                model.item.image = ImageConverter.decodeBase64ToByteArray(model.ImagePNGbase64);
            }
            

            //reset and check the model state
            ModelState.Clear();
            TryValidateModel(model);

            string action = TempData["action"].ToString();
            if (ModelState.IsValid)
            {
                if (action == "Add")
                {
                    data.Items.Insert(model.item);
                    data.Save();
                    return RedirectToAction("ListUserCollections", "Collection");
                }
                else if(action == "Edit")
                {
                    Item item = data.Items.Get(ItemId);
                    item.Name = model.item.Name;
                    item.Description = model.item.Description;
                    item.image = model.item.image;
                    data.Items.Update(item);
                    data.Save();
                    return RedirectToAction("ListUserCollections", "Collection");
                }
                else
                {
                    if (action == "Edit")
                    {
                        TempData["action"] = "Edit";
                        model.Edit = true;   
                    }
                    else
                    {
                        TempData["action"] = "Add";
                    }
                    TempData["CollectionId"] = CollectionId;
                    TempData["ItemId"] = ItemId;
                    return View(model);
                }
            }
            else
            {
                if (action == "Edit")
                {
                    TempData["action"] = "Edit";
                    model.Edit = true;
                }
                else
                {
                    TempData["action"] = "Add";
                }
                TempData["CollectionId"] = CollectionId;
                TempData["ItemId"] = ItemId;
                return View(model);
            }
        }
        public IActionResult Details(int id)
        {
            ItemViewModel model=new ItemViewModel();
            model.item=data.Items.Get(id);
            model.ImagePNGbase64=ImageConverter.byteArrayTo64BaseEncode(model.item.image);
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id) {
            return View();
        }
        [HttpPost]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
