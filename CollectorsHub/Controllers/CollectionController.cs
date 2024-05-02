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

        public IActionResult List(int id,string filter)
        {
            CollectionViewModel model = new CollectionViewModel();
            if (filter != "All" && filter !=null)
            {
                model.Collection = data.Collections.List(new QueryOptions<Collection>
                {
                    Include = "Items",
                    Where = (col => col.CollectionId == id)
                }).Select(col =>
                {
                    col.Items = col.Items.Where(itm => itm.Name.Contains(filter)).ToList();
                    return col;
                }).FirstOrDefault();
            }
            else
            {
                model.Collection = data.Collections.List(new QueryOptions<Collection>
                {
                    Include = "Items",
                    Where = (col => col.CollectionId == id)
                }).FirstOrDefault();
            }

            return View(model);
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

        public IActionResult Add()
        {
            CollectionViewModel model = new CollectionViewModel();
            TempData["action"] = "Add";
            return View("AddEdit",model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            CollectionViewModel model = new CollectionViewModel();
            TempData["action"] = "Edit";
            model.Collection = data.Collections.Get(id);
            model.Edit = true;
            return View("AddEdit",model);
        }
        [HttpPost]
        public IActionResult AddEdit(CollectionViewModel model)
        {
            model.Collection.UserId = data.Users.Get(new QueryOptions<User>
            {
                Where = (user => user.UserName == User.Identity.Name)
            }).Id;
            model.Collection.User = data.Users.Get(model.Collection.UserId);
            string action = TempData["action"].ToString();
            ModelState.Clear();
            TryValidateModel(model);

                if (ModelState.IsValid) {
                    if (action=="Edit")
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
                TempData["action"] = action;
                if (action == "Edit")
                {
                    
                    model.Edit = true;
                }
                return View("AddEdit",model);
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
        public IActionResult filter(int id,string filter)
        {
            if (filter == null)
            {
                filter = "All";
            }
            string customRoute = "List/"+id+"/filter/" + filter;
            return Redirect(customRoute);
        }

    }
}
