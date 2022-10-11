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
            try
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
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while getting your Profile. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Home");
            }
            TempData["NotFoundIndex"] = "Cannot find your profile. Please contact support to fix this issue";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateProfile()
        {
              ApplicationUser user = await userManager.GetUserAsync(User);
            var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.UserId == user.Id);
            //var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.UserId == "1923");
            if (profile != null)
              {
                  return View(profile);
              }
            TempData["NotFoundProfile"] = "Cannot find your profile. Please contact support to fix the issue";
              return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int id, [Bind("Id, UserId, FirstName, LastName, Gender, DateOfBirth, IsSubscribed")] ProfileModel profile)
        {
            try
            {
                if (id != profile.Id)
                {
                    return NotFound();
                }
                if (profile.DateOfBirth != null)
                {
                    if (!IsDateValid((DateTime)profile.DateOfBirth))
                    {
                        ModelState.AddModelError("DateOfBirth", "Date cannot be in the future");
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Profile Updated Successfully";
                    return RedirectToAction("Index", "Profile");
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while updating your profile. Please try again later. " + x.GetBaseException().Message;
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
            TempData["NotFoundProfile"] = "Cannot find your preference. Please contact support to fix the issue";
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePreference([Bind("PreferencesModelId, UserId, Platform, Category")] PreferenceViewModel preference)
        {
            try
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
                    TempData["Success"] = "Preference Updated Successfully";
                    return RedirectToAction("Index", "Profile");
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while updating your preference. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
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
            TempData["NotFoundProfile"] = "Cannot find your address. Please contact support to fix the issue";
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress([Bind("AddressModelId, UserId, MailingAddress, ShippingAddress, IsSame")] AddressModel address)
        {
            try
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
                    addr.ShippingAddress = address.IsSame ? address.MailingAddress : address.ShippingAddress;
                    addr.IsSame = address.IsSame;
                    _context.Update(addr);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Preference Updated Successfully";
                    return RedirectToAction("Index", "Profile");
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while updating your address. Please try again later. " + x.GetBaseException().Message;
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
