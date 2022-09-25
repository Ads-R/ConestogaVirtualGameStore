using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace ConestogaVirtualGameStore.Controllers
{
    public class EmailController : Controller
    {
        private UserManager<ApplicationUser> userManager;

        public EmailController(UserManager<ApplicationUser> uManager)
        {
            userManager = uManager;
        }

        public async Task<IActionResult> ConfirmedEmail(string token, string email)
        {
            try
            {

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return View("Error");

                var result = await userManager.ConfirmEmailAsync(user, token);
                return View(result.Succeeded ? "ConfirmedEmail" : "Error");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error in EmailController " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
