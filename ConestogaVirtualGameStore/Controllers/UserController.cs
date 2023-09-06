using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
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
        private readonly GameStoreContext _context;
        private readonly IConfiguration configuration;

        public UserController(UserManager<ApplicationUser> uManager, SignInManager<ApplicationUser> sManager,
            IPasswordHasher<ApplicationUser> passwordHash, IPasswordValidator<ApplicationUser> passwordV, GameStoreContext context, IConfiguration config)
        {
            userManager = uManager;
            signInManager = sManager;
            passwordHasher = passwordHash;
            passwordValidator = passwordV;
            _context = context;
            configuration = config;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter the captcha as a number.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                    };
                    IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                    //throw new Exception("Testing the exception");
                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
                        var confirmationLink = Url.Action("ConfirmedEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                        EmailSender emailSender = new EmailSender(configuration);
                        bool emailResponse = emailSender.SendEmail(user.Email, confirmationLink);

                        if (emailResponse)
                        {
                            ProfileModel profile = new ProfileModel
                            {
                                UserId = appUser.Id,
                                Gender = Gender.None
                            };
                            PreferencesModel preference = new PreferencesModel
                            {
                                UserId = appUser.Id,
                            };
                            AddressModel address = new AddressModel
                            {
                                UserId = appUser.Id,
                            };
                            _context.Add(profile);
                            _context.Add(preference);
                            _context.Add(address);
                            await _context.SaveChangesAsync();
                            MailingAddress mail = new MailingAddress
                            {
                                AddressModelId = address.AddressModelId
                            };
                            ShippingAddress ship = new ShippingAddress
                            {
                                AddressModelId = address.AddressModelId
                            };
                            _context.Add(mail);
                            _context.Add(ship);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("ConfirmEmail", "User");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed sending email confirmation. Please try again");
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
                catch (Exception x)
                {
                    TempData["ExceptionMessage"] = "An unexpected error has occurred while registering. Please try again later. " + x.GetBaseException().Message;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(user);
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
                try
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
                catch (Exception x)
                {
                    TempData["ExceptionMessage"] = "An unexpected error has occurred while Logging in. Please try again later. " + x.GetBaseException().Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                await signInManager.SignOutAsync();
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while Logging out. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel passwordModel)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    //I used a passwordValidator here because Identity does not automatically apply the password policy
                    //when updating user, only on creating user
                    if (ModelState.IsValid)
                    {
                        var resultValidate = passwordValidator.ValidateAsync(userManager, user, passwordModel.NewPassword);
                        if (!resultValidate.Result.Succeeded)
                        {
                            foreach (IdentityError err in resultValidate.Result.Errors)
                            {
                                ModelState.AddModelError("", err.Description);
                            }
                        }
                        user.PasswordHash = passwordHasher.HashPassword(user, passwordModel.NewPassword);
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            TempData["ChangeSuccess"] = "Password has been changed successfully.";
                            return View();
                        }
                        foreach (IdentityError err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while Changing Password. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel emailAccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await _context.Users.Where(a => a.Email == emailAccount.Email).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        string newPassword = PasswordGenerator(12);
                        user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            EmailSender emailSender = new EmailSender(configuration);
                            bool emailResponse = emailSender.SendEmailPassword(user.Email, newPassword);
                            if (!emailResponse)
                            {
                                ModelState.AddModelError("", "An error occured, please try resetting your password again later");
                            }
                            TempData["ResetSuccess"] = "Password has been reset successfully. Check your email for your new password";
                            return View();
                        }
                        ModelState.AddModelError("", "Password reset failed, please try again later");
                        return View();
                    }
                    ModelState.AddModelError("", "Email not found");
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while Resetting Password. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public string PasswordGenerator(int passwordLength)
        {
            bool HasLowerCase = false;
            bool HasUpperCase = false;
            bool HasDigit = false;
            bool HasSpecialCharacter = false;
            Random random = new Random();
            StringBuilder newPasssword = new StringBuilder();
            try
            {
                while (passwordLength > newPasssword.Length)
                {
                    char character = (char)random.Next(32, 126);
                    newPasssword.Append(character);
                    if (char.IsLower(character))
                    {
                        HasLowerCase = true;
                    }
                    else if (char.IsUpper(character))
                    {
                        HasUpperCase = true;
                    }
                    else if (char.IsDigit(character))
                    {
                        HasDigit = true;
                    }
                    else if (!char.IsLetterOrDigit(character))
                    {
                        HasSpecialCharacter = true;
                    }
                }

                if (!HasLowerCase)
                {
                    newPasssword.Append((char)random.Next(97, 123));
                }
                if (!HasUpperCase)
                {
                    newPasssword.Append((char)random.Next(65, 91));
                }
                if (!HasDigit)
                {
                    newPasssword.Append((char)random.Next(48, 58));
                }
                if (!HasSpecialCharacter)
                {
                    newPasssword.Append((char)random.Next(33, 48));
                }
                return newPasssword.ToString();
            }
            catch (Exception x)
            {
                throw new Exception("Error occured while generating a new password");
            }
        }
    }
}
