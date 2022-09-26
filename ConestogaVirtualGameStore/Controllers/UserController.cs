using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IPasswordValidator<ApplicationUser> passwordValidator;

        public UserController(UserManager<ApplicationUser> uManager, SignInManager<ApplicationUser> sManager, 
            IPasswordHasher<ApplicationUser> passwordHash, IPasswordValidator<ApplicationUser> passwordV)
        {
            userManager = uManager;
            signInManager = sManager;
            passwordHasher = passwordHash;
            passwordValidator = passwordV;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                    };
                    IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
                        var confirmationLink = Url.Action("ConfirmedEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                        EmailSender emailSender = new EmailSender();
                        bool emailResponse = emailSender.SendEmail(user.Email, confirmationLink);
                        
                        if (emailResponse)
                        {
                            return RedirectToAction("ConfirmEmail", "User");
                        }
                        else
                        {
                            System.Console.WriteLine("Send Email Failed");
                        }
                    }
                    else
                    {
                        foreach (IdentityError err in result.Errors)
                        {
                            ModelState.AddModelError("", "Account Registration Failed: " + err.Description);
                        }
                    }
                }
                return View(user);
            }
            catch (Exception x)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [AllowAnonymous]
        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.UserName, user.Password, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account has been locked due to consecutive " +
                        "failed logins, Please wait 5 minutes before trying again");
                }
                ModelState.AddModelError("", "Invalid Login: ");
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel passwordModel)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                var resultValidate = passwordValidator.ValidateAsync(userManager, user, passwordModel.NewPassword);
                if (!resultValidate.Result.Succeeded)
                {
                    foreach (IdentityError err in resultValidate.Result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
                if (ModelState.IsValid)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, passwordModel.NewPassword);
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View();
        }
    }
}
