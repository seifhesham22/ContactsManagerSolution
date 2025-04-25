using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDEXAMPLE.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return View(register);
            }
            ApplicationUser user = new ApplicationUser() { PersonName = register.PersonName, UserName = register.Email, Email = register.Email, PhoneNumber = register.Phone };
            IdentityResult result = await _userManager.CreateAsync(user , register.Password);
            if (result.Succeeded)
            {
                if (register.UserType == Core.Enums.UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole Role = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(Role);
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }   
                else
                {
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register" , error.Description);
                }
            }
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login , string? returnUrl)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return View(login);
            }
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                ApplicationUser? user =await _userManager.FindByEmailAsync(login.Email);
                if(user != null)
                {
                    if(await _userManager.IsInRoleAsync(user , UserTypeOptions.Admin.ToString()) == true)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                if(!returnUrl.IsNullOrEmpty() && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            ModelState.AddModelError("Login", "Wrong Email or Password");
            return View(login); 
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
