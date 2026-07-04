using GymManagment.BLL.ViewModels.LoginVMs;
using GymManagment.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager , 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // Get :: Login => Empty form
        [HttpGet]
        public IActionResult Login()            
            => View();

        // Post :: Sign in
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invaild Email");
                return View(model);
            }

            // sign in

            var result = await _signInManager.PasswordSignInAsync(user,model.Password , model.RememberMe , false) ;

            if (result.Succeeded)
            {
                //_logger.LogInformation($"User {user.UserName} Logged In");
                return RedirectToAction( nameof(HomeController.Index) ,"Home");
            }
            else if (result.IsLockedOut)
            {
                //_logger.LogWarning($"User {user.UserName} Locked Out");
                ModelState.AddModelError("InvalidLogin", "This Account Locked Out . Try Again Later");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidLogin", "Invaild Email or Password");
                return View(model);
            }
        }

        // Shows friendly message Access Denied
        [HttpGet]
        public IActionResult AccessDenied()
            => View();


        // sign out
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }            
    }
}
