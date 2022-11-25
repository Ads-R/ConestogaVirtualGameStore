using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    var records = await _wishService.GetAllGames(user.Id);
                    return View(records);
                }
                return NotFound();

            }
            catch (System.Exception)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddWishList(int GameId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await userManager.GetUserAsync(User);
                    _wishService.AddWishList(user.Id, GameId);
                    TempData["WishAdd"] = "Game added to wish list";
                }
                return Redirect($"/Home/GameDetails/{GameId}");
            }

            catch (System.Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult>DeleteWish(int wishId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _wishService.RemoveWishList(user.Id, wishId);
                TempData["WishSuccess"] = "Game removed successfully";
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                TempData["WishFail"] = "\"An error has occurred while trying to remove a game. Please try again later.\"";
                return RedirectToAction("Index");
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFriendWishList(string friendUserName)
        {
            try
            {
                var friendRecord = await _wishService.GetFriendGames(friendUserName);
                return View("FriendWish",friendRecord);
            }
            catch (System.Exception)
            {
                throw;
            }
            //var records = await _wishService.GetAllGames(friendUserName);
            //return View("FriendWish");
        }
    }
}
