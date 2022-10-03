using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GameStoreContext _context;
        public ProfileController(UserManager<ApplicationUser> uManager, GameStoreContext context)
        {
            userManager = uManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (profile != null)
            {
                return View(profile);
            }
            return RedirectToAction("Index", "Profile");
        }

        public async Task<IActionResult> UpdateProfile()
        {
              ApplicationUser user = await userManager.GetUserAsync(User);
              var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.UserId == user.Id);
              if (profile != null)
              {
                  return View(profile);
              }
              return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int id, [Bind("Id, UserId, FirstName, LastName, Gender, DateOfBirth, IsSubscribed")] ProfileModel profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }
            if (profile.DateOfBirth != null)
            {
                bool valid = IsDateValid((DateTime)profile.DateOfBirth);
                if (!valid)
                {
                    ModelState.AddModelError("DateOfBirth","Date cannot be in the future");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Update(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Profile");
            }
            return View("UpdateProfile", profile);
        }

        [HttpPost]
        [AllowAnonymous]
        public bool IsDateValid(DateTime dob)
        {
            if(dob > DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
