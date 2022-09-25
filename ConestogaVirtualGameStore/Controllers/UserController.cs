﻿using ConestogaVirtualGameStore.Models;
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
                        return RedirectToAction("ConfirmEmail", "User");
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
                var result = await signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
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
