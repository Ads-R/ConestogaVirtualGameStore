using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> uManager, SignInManager<ApplicationUser> sManager)
        {
            userManager = uManager;
            signInManager = sManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
    }
}
