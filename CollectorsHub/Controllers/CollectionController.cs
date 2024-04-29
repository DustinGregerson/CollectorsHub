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
                User user = data.Users.Get(new QueryOptions<User>{
                    Where = (u => u.UserName==User.Identity.Name)
                });

                model.Collection = data.Collections.Get(new QueryOptions<Collection>
                {
                    Where = (col => col.UserId == user.Id)
                });
                model.Edit = true;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult AddEdit(CollectionViewModel model)
        {
                if (model.Collection.Name != "" && model.Collection.Tag != "")
                {
                    if (model.Edit)
                    {
                        model.Collection.UserId = data.Users.Get(new QueryOptions<User>
                        {
                            Where = (user => user.UserName == User.Identity.Name)
                        }).Id;
                    data.Collections.Update(model.Collection);
                    data.Save();
                    return RedirectToAction("ListUserCollections");
                }
                else
                    {
                        model.Collection.UserId = data.Users.Get(new QueryOptions<User>
                        {
                            Where = (user => user.UserName == User.Identity.Name)
                        }).Id;

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
