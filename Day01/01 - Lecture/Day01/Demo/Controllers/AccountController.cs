using Demo.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;

namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
                ModelState.AddModelError("Email", "Email Exists!");
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(model, model.PasswordHash);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(model, "User");
                    return RedirectToAction("login", "account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var res = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (res.Succeeded)
                    return RedirectToAction("index", "home");
                ModelState.AddModelError("", "Invalid UserName Or Password!");
            }
            return View(model);

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
 