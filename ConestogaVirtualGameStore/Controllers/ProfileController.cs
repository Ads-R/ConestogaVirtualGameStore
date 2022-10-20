using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
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
                var address = await _context.Address
                    .Include(a => a.MailingAddress.Country)
                    .Include(b => b.MailingAddress.Province)
                    .Include(e => e.MailingAddress.City)
                    .Include(c => c.ShippingAddress.Country)
                    .Include(d => d.ShippingAddress.Province)
                    .Include(f => f.ShippingAddress.City)
                    .FirstOrDefaultAsync(b => b.UserId == user.Id);
                if (profile != null)
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
       /* public async Task<IActionResult> Profile()
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                var profile = await _context.Profiles.FirstOrDefaultAsync(b => b.UserId == user.Id);
                if (profile != null)
                {
                    return View(profile);
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while getting your Profile. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
            TempData["NotFoundProfile"] = "Cannot find your Profile. Please contact support to fix this issue";
            return RedirectToAction("Index", "Profile");
        }

        public async Task<IActionResult> Preference()
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                var preferences = await _context.Preferences.FirstOrDefaultAsync(b => b.UserId == user.Id);
                if(preferences != null)
                {
                    return View(preferences);
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while getting your Preference. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
            TempData["NotFoundProfile"] = "Cannot find your Preference. Please contact support to fix this issue";
            return RedirectToAction("Index", "Profile");
        }

        public async Task<IActionResult> Address()
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                var address = await _context.Address.FirstOrDefaultAsync(b => b.UserId == user.Id);
                if (address != null)
                {
                    return View(address);
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while getting your Address. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
            TempData["NotFoundProfile"] = "Cannot find your Address. Please contact support to fix this issue";
            return RedirectToAction("Index", "Profile");
        } */

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
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                var address = await _context.Address
                    .Include(b => b.MailingAddress)
                    .Include(c => c.ShippingAddress)
                    .FirstOrDefaultAsync(a => a.UserId == user.Id);

                List<Country> countryList = new List<Country>();
                List<Province> mailProvinceList = new List<Province>();
                List<City> mailCityList = new List<City>();

                List<Province> shipProvinceList = new List<Province>();
                List<City> shipCityList = new List<City>();

                countryList = await _context.Country.ToListAsync();
                countryList.Insert(0, new Country { CountryId = 0, CountryName = "Select Province" });

                if (address.MailingAddress.MailCountry != null)
                {
                    mailProvinceList = await _context.Province.Where(e => e.CountryId == address.MailingAddress.MailCountry).ToListAsync();
                }
                if (address.ShippingAddress.ShipCountry != null)
                {
                    shipProvinceList = await _context.Province.Where(e => e.CountryId == address.ShippingAddress.ShipCountry).ToListAsync();
                }
                if (address.MailingAddress.MailProvince != null)
                {
                    mailCityList = await _context.City.Where(e => e.ProvinceId == address.MailingAddress.MailProvince).ToListAsync();
                }
                if (address.ShippingAddress.ShipProvince != null)
                {
                    shipCityList = await _context.City.Where(e => e.ProvinceId == address.ShippingAddress.ShipProvince).ToListAsync();
                }
                if (address != null)
                {
                    mailProvinceList.Insert(0, new Province { ProvinceId = 0, ProvinceName = "Select Province" });
                    shipProvinceList.Insert(0, new Province { ProvinceId = 0, ProvinceName = "Select Province" });
                    mailCityList.Insert(0, new City { CityId = 0, CityName = "Select City" });
                    shipCityList.Insert(0, new City { CityId = 0, CityName = "Select City" });
                    ViewBag.MailCity = mailCityList;
                    ViewBag.MailProvince = mailProvinceList;
                    ViewBag.ShipCity = shipCityList;
                    ViewBag.ShipProvince = shipProvinceList;
                    ViewBag.Country = countryList;
                    return View(address);
                }
                TempData["NotFoundProfile"] = "Cannot find your address. Please contact support to fix the issue";
                return RedirectToAction("Index", "Profile");
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while querying your address data. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(AddressModel address)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                AddressModel addr = await _context.Address
                    .Include(b => b.MailingAddress)
                    .Include(c => c.ShippingAddress)
                    .Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
                if (addr == null)
                {
                    return NotFound();
                }
                if (addr.UserId != address.UserId || addr.AddressModelId != address.AddressModelId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    addr.IsSame = address.IsSame;
                    MailingAddress mail = addr.MailingAddress;
                    ShippingAddress ship = addr.ShippingAddress;
                    string address1 = address.MailingAddress.MailAddress1;
                    string address2 = address.MailingAddress.MailAddress2;
                    int? CountryId = address.MailingAddress.MailCountry == 0 ? null : address.MailingAddress.MailCountry;
                    int? ProvinceId = address.MailingAddress.MailProvince == 0 ? null : address.MailingAddress.MailProvince;
                    int? CityId = address.MailingAddress.MailCity == 0 ? null : address.MailingAddress.MailCity;
                    mail.MailAddress1 = address1;
                    mail.MailAddress2 = address2;
                    mail.MailCountry = CountryId;
                    mail.MailProvince = ProvinceId;
                    mail.MailCity = CityId;
                    if (address.IsSame)
                    {
                        ship.ShipAddress1 = address1;
                        ship.ShipAddress2 = address2;
                        ship.ShipCountry = CountryId;
                        ship.ShipProvince = ProvinceId;
                        ship.ShipCity = CityId;
                    }
                    else
                    {
                        ship.ShipAddress1 = address.ShippingAddress.ShipAddress1;
                        ship.ShipAddress2 = address.ShippingAddress.ShipAddress2;
                        ship.ShipCountry = address.ShippingAddress.ShipCountry == 0 ? null : address.ShippingAddress.ShipCountry;
                        ship.ShipProvince = address.ShippingAddress.ShipProvince == 0 ? null : address.ShippingAddress.ShipProvince;
                        ship.ShipCity = address.ShippingAddress.ShipCity == 0 ? null : address.ShippingAddress.ShipCity;
                    }
                    _context.Update(addr);
                    _context.Update(mail);
                    _context.Update(ship);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Address Updated Successfully";
                    return RedirectToAction("Index", "Profile");
                }
            }
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while updating your address. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }

            List<Country> countryList = new List<Country>();
            List<Province> mailProvinceList = new List<Province>();
            List<City> mailCityList = new List<City>();

            List<Province> shipProvinceList = new List<Province>();
            List<City> shipCityList = new List<City>();

            countryList = await _context.Country.ToListAsync();
            countryList.Insert(0, new Country { CountryId = 0, CountryName = "Select Province" });

            if (address.MailingAddress.MailCountry != null)
            {
                mailProvinceList = await _context.Province.Where(e => e.CountryId == address.MailingAddress.MailCountry).ToListAsync();
            }
            if (address.ShippingAddress.ShipCountry != null)
            {
                shipProvinceList = await _context.Province.Where(e => e.CountryId == address.ShippingAddress.ShipCountry).ToListAsync();
            }
            if (address.MailingAddress.MailProvince != null)
            {
                mailCityList = await _context.City.Where(e => e.ProvinceId == address.MailingAddress.MailProvince).ToListAsync();
            }
            if (address.ShippingAddress.ShipProvince != null)
            {
                shipCityList = await _context.City.Where(e => e.ProvinceId == address.ShippingAddress.ShipProvince).ToListAsync();
            }
                mailProvinceList.Insert(0, new Province { ProvinceId = 0, ProvinceName = "Select Province" });
                shipProvinceList.Insert(0, new Province { ProvinceId = 0, ProvinceName = "Select Province" });
                mailCityList.Insert(0, new City { CityId = 0, CityName = "Select City" });
                shipCityList.Insert(0, new City { CityId = 0, CityName = "Select City" });
                ViewBag.MailCity = mailCityList;
                ViewBag.MailProvince = mailProvinceList;
                ViewBag.ShipCity = shipCityList;
                ViewBag.ShipProvince = shipProvinceList;
                ViewBag.Country = countryList;

                return View("UpdateAddress", address);
        }
        /* [HttpPost]
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
                    //addr.ShippingAddress = address.IsSame ? address.MailingAddress : address.ShippingAddress;
                    addr.IsSame = address.IsSame;
                    _context.Update(addr);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Preference Updated Successfully";
                    return RedirectToAction("Address", "Profile");
                }
            } 
            catch (Exception x)
            {
                TempData["ExceptionMessage"] = "An unexpected error has occurred while updating your address. Please try again later. " + x.GetBaseException().Message;
                return RedirectToAction("Index", "Profile");
            }
            return View("UpdateAddress", address);
        } */

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

        public JsonResult GetProvince(int CountryId)
        {
            List<Province> provinces = new List<Province>();
            provinces = _context.Province.Where(p => p.CountryId == CountryId).ToList();
            provinces.Insert(0, new Province { ProvinceId = 0, ProvinceName = "Select Province" });
            return Json(new SelectList(provinces, "ProvinceId", "ProvinceName"));
        }

        public JsonResult GetCity(int ProvinceId)
        {
            List<City> cities = new List<City>();
            cities = _context.City.Where(p => p.ProvinceId == ProvinceId).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City" });
            return Json(new SelectList(cities, "CityId", "CityName"));
        }
    }
}
