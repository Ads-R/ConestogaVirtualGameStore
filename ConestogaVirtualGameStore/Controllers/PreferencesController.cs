using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class PreferencesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GameStoreContext _context;

        public PreferencesController(UserManager<ApplicationUser> uManager, GameStoreContext context)
        {
            userManager = uManager;
            _context = context;
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

                pref.Category =  preference.Category != null ? preference.Category.Join(",") : null;
                pref.Platform = preference.Platform != null ? preference.Platform.Join(",") : null;
                _context.Update(pref);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
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
    }
}
