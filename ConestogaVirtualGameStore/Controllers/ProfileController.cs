using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
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
            var preferences = await _context.Preferences.FirstOrDefaultAsync(b => b.UserId == user.Id);
            var address = await _context.Address.FirstOrDefaultAsync(b => b.UserId == user.Id);
            if (profile != null && preferences != null)
            {
                ProfilePreferenceAddressViewModel profPref = new ProfilePreferenceAddressViewModel
                {
                    Profile = profile,
                    Preferences = preferences,
                    Address = address
                };
                return View(profPref);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateProfile()
        {
              ApplicationUser user = await userManager.GetUserAsync(User);
              var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.UserId == user.Id);
              if (profile != null)
              {
                  return View(profile);
              }
              return RedirectToAction("Index", "Home");
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
                if (!IsDateValid((DateTime)profile.DateOfBirth))
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

        public async Task<IActionResult> UpdatePreference()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var pref = await _context.Preferences.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (pref != null)
            {
                PreferenceViewModel preference = new PreferenceViewModel
                {
                    PreferencesModelId = pref.PreferencesModelId,
                    UserId = pref.UserId,
                    Category = pref.Category != null ? pref.Category.Split(",") : null,
                    Platform = pref.Platform != null ? pref.Platform.Split(",") : null
                };
                return View(preference);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePreference([Bind("PreferencesModelId, UserId, Platform, Category")] PreferenceViewModel preference)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            PreferencesModel pref = await _context.Preferences.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
            if (pref.UserId != preference.UserId && pref.PreferencesModelId != preference.PreferencesModelId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                pref.Category = preference.Category != null ? preference.Category.Join(",") : null;
                pref.Platform = preference.Platform != null ? preference.Platform.Join(",") : null;
                _context.Update(pref);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Profile");
            }
            /* var id = _context.Preferences.Where(a => a.UserId == user.Id).AsNoTracking().FirstOrDefault();
            if(user.Id != preference.UserId && id.PreferencesModelId != preference.PreferencesModelId)
            {
                return NotFound();
            } 
            if (ModelState.IsValid)
            {
                PreferencesModel pref = new PreferencesModel
                {
                    PreferencesModelId = preference.PreferencesModelId,
                    UserId = preference.UserId,
                    Category = preference.Category != null ? preference.Category.Join(",") : null,
                    Platform = preference.Platform != null ? preference.Platform.Join(",") : null
                }; 
                _context.Update(pref);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); 
            } */
            return View("UpdatePreference", preference);
        }

        public async Task<IActionResult> UpdateAddress()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var address = await _context.Address.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (address != null)
            {
                return View(address);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress([Bind("AddressModelId, UserId, MailingAddress, ShippingAddress")] AddressModel address)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            AddressModel addr = await _context.Address.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
            if (addr.UserId != address.UserId && addr.AddressModelId != address.AddressModelId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                addr.MailingAddress = address.MailingAddress;
                addr.ShippingAddress = address.ShippingAddress;
                _context.Update(addr);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Profile");
            }
            return View("UpdateAddress", address);
        }

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
