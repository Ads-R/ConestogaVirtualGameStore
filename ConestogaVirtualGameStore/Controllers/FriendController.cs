using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly UserManager<ApplicationUser> userManager;
        public FriendController(IFriendService friendService, UserManager<ApplicationUser> uManager)
        {
            _friendService = friendService;
            userManager = uManager;
        }
        public async Task<IActionResult> Index(string search, string ise)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var searchResults = Enumerable.Empty<ApplicationUser>().AsQueryable();
            if (!String.IsNullOrEmpty(search))
            {
                searchResults = _friendService.SearchUsers(search, user.UserName);
                //Excalamation point is a null forgiving operator for the compiler
                //game = game.Where(a => a.Title!.Contains(search));

            }

            var all = await _friendService.GetAllFriends(user.Id);
            var accepted = await _friendService.GetAllAcceptedFriends(user.Id);
            var pendingApproval = await _friendService.GetPendingApproval(user.Id);
            var pendingRequest = await _friendService.GetPendingRequest(user.Id);
            FriendSearchViewModel friend = new FriendSearchViewModel()
            {
                UserId = user.Id,
                Users = searchResults.ToList(),
                AllFriends = all.ToList(),
                AcceptedFriends = accepted.ToList(),
                PendingApproval = pendingApproval.ToList(),
                PendingRequest = pendingRequest.ToList(),
                SearchString = search
            };
            ViewBag.IsSearching = ise;
            return View(friend);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string userId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _friendService.AddFriend(user.Id, userId);
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["FException"] = x.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFriend(string friendId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _friendService.DeleteFriend(user.Id, friendId);
                return RedirectToAction("Index");
            }
            catch(Exception x)
            {
                TempData["FException"] = x.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AcceptFriend(string requesterId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _friendService.AcceptRequest(user.Id, requesterId);
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["FException"] = x.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> DeclineFriend(string requesterId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _friendService.DeclineRequest(user.Id, requesterId);
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["FException"] = x.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CancelRequest(string friendId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _friendService.CancelRequest(user.Id, friendId);
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["FException"] = x.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
