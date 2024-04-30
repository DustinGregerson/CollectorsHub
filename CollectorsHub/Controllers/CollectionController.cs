using CollectorsHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CollectorsHub.Controllers
{
    public class CollectionController : Controller
    {
        CollectorsHubUnitOfWork data;
        public CollectionController(CollectorsHubContext ctx) => data = new CollectorsHubUnitOfWork(ctx);

        public IActionResult List(int id)
        {
            Collection collection = data.Collections.Get(new QueryOptions<Collection>
            {
                Include = "Items",
                Where = (col => col.CollectionId == id)
            });

            return View(collection);
        }
        public IActionResult ListUserCollections()
        {
            string userName = User.Identity.Name;
            User user = data.Users.Get(new QueryOptions<User>
            {
                Where = (u=>u.UserName==userName)
            });


            List<Collection> collection = data.Collections.List(new QueryOptions<Collection>
            {
                Include = "Items",
                Where = (col => col.UserId == user.Id)
            }).ToList();

            CollectionViewModel model = new CollectionViewModel()
            {
                UserCollections = collection
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult AddEdit(int id)
        {
            CollectionViewModel model = new CollectionViewModel();

            if (id > 0)
            {
                TempData["action"] = "Edit";
                model.Collection = data.Collections.Get(id);
                model.Edit = true;
                return View(model);
            }

            TempData["action"] = "Add";
            return View(model);
        }
        [HttpPost]
        public IActionResult AddEdit(CollectionViewModel model)
        {
            model.Collection.UserId = data.Users.Get(new QueryOptions<User>
            {
                Where = (user => user.UserName == User.Identity.Name)
            }).Id;
            model.Collection.User = data.Users.Get(model.Collection.UserId);

            ModelState.Clear();

            TryValidateModel(model);

                if (ModelState.IsValid) {
                    if (TempData["action"].ToString()=="Edit")
                    {
                    data.Collections.Update(model.Collection);
                    data.Save();
                        return RedirectToAction("ListUserCollections");
                    }
                    else
                    {
                        data.Collections.Insert(model.Collection);
                        data.Save();
                        return RedirectToAction("ListUserCollections");
                    }
                }
                else
                {
                    return RedirectToAction("AddEdit");
                }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            CollectionViewModel model=new CollectionViewModel();
            model.Collection=data.Collections.Get(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(CollectionViewModel model)
        {
            data.Collections.Delete(data.Collections.Get(model.Collection.CollectionId));
            data.Collections.Save();
            return RedirectToAction("ListUserCollections");
        }

    }
}
