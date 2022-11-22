using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]

    public class WishListController : Controller
    {
        private readonly IWish _wishService;
        private readonly UserManager<ApplicationUser> userManager;
        public WishListController(IWish wishService, UserManager<ApplicationUser> uManager)
        {
            _wishService = wishService;
            userManager = uManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
