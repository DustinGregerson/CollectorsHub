using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CollectorsHub.Controllers
{

    public class ItemController : Controller
    {
        CollectorsHubUnitOfWork data;
        public ItemController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult List(int id)
        {
            ItemViewModel model=new ItemViewModel();
            model.collection = data.Collections.Get(id);
            return View(model);
        }

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
            string action = TempData["action"].ToString();

            //The model state is set to the point to were the item model is the only object that
            //could have invalid field data so the asp validation summery gives the correct errors

            int CollectionId = int.Parse(TempData["CollectionId"].ToString());
            model.item.CollectionId = CollectionId;
            model.item.Collection = data.Collections.Get(CollectionId);
            model.item.Collection.User = data.Users.Get(new QueryOptions<User>
            {
                Where = (user => user.UserName == User.Identity.Name)
            });
            

            //This section checks to see if an img file was passed to the model.
            //if not an attempt is made to see if the model contains an image
            //if i doesn't contain one then the model state will be invalid

            if (model.imgFile != null)
            {
                model.item.image = ImageConverter.iFormImageFileToByteArray(model.imgFile);
            }
            else if (model.imgFile == null && model.ImagePNGbase64 != null)
            {
                model.item.image = ImageConverter.decodeBase64ToByteArray(model.ImagePNGbase64);
            }


            //reset and check the model state
            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
            {
                if (action == "Add")
                {
                    //inserting new item into collection
                    data.Items.Insert(model.item);
                    data.Save();
                    return RedirectToAction("ListUserCollections", "Collection");
                }
                else if (action == "Edit")
                {
                    //The item that needs to be updated is retrieved from from database and then modified
                    //with the models item values. If you are wondering why i am not using th model item
                    //to update the database it is because EF core's Navigation tracking mechanic was getting
                    //confused.
                    int ItemId = int.Parse(TempData["ItemId"].ToString());
                    Item item = data.Items.Get(ItemId);
                    item.Name = model.item.Name;
                    item.Description = model.item.Description;
                    item.image = model.item.image;
                    data.Items.Update(item);
                    data.Save();
                    return RedirectToAction("ListUserCollections", "Collection");
                }
                // if the model state is valid but somehow the user managed to bypass the [Get] AddEdit
                //IAction then this section is called.
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
                    
                    int ItemId = int.Parse(TempData["ItemId"].ToString());
                    TempData["CollectionId"] = CollectionId;
                    TempData["ItemId"] = ItemId;
                    return View(model);
                }
            }
            //In this case the models state is invalid
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
                
                int ItemId = int.Parse(TempData["ItemId"].ToString());
                TempData["CollectionId"] = CollectionId;
                TempData["ItemId"] = ItemId;
                return View(model);
            }
        }
        public IActionResult Details(int itemId)
        {
            ItemViewModel model=new ItemViewModel();
            model.item=data.Items.Get(itemId);
            model.ImagePNGbase64=ImageConverter.byteArrayTo64BaseEncode(model.item.image);
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int itemId)
        {
            ItemViewModel model = new ItemViewModel();
            model.item = data.Items.Get(itemId);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(ItemViewModel model)
        {
            Item item = data.Items.Get(model.item.itemId);
            data.Items.Delete(item);
            data.Save();
            return Redirect("/Collection/ListUserCollections");
        }
    }
}
