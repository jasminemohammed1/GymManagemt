using GymManagement.BLL.ViewModels.AccountViewModel;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;

namespace GymManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinmanager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> usermanager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signinmanager = usermanager;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);
            }
            var res = await signinmanager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (res.Succeeded)
            {
                logger.LogInformation($"user with name {user.UserName} is logged");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (res.IsLockedOut)
            {
                logger.LogWarning($"user with name {user.UserName} is locked out try again later");
                ModelState.AddModelError("InvalidLogin", "Account is locked out try agian later");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidError", "Invalid Email or Password");
                return View(model);
            }


        }

        [HttpPost]
        [Authorize]
        public async Task< IActionResult> Logout()
        {

            await signinmanager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    } 

}
