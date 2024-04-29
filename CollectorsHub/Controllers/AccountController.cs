using CollectorsHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CollectorsHub.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userMngr,
            SignInManager<User> signInMngr)
        {
            userManager = userMngr;
            signInManager = signInMngr;
        }
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new CollectorsHub.Models.User
                {
                    UserName = model.UserName
                    ,
                    firstName = model.FirstName
                    ,
                    lastName = model.LastName
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("userName",model.UserName);
                    await userManager.AddToRoleAsync(user, model.RoleName);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            return View(accountViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Username, model.Password, true,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("userName", model.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.SetString("userName", "");
            return RedirectToAction("Index", "Home");
        }
    }
}
