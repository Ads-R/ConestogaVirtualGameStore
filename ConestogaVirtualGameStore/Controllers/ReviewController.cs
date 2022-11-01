using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GameStoreContext _context;
        public ReviewController(GameStoreContext context, UserManager<ApplicationUser> uManager)
        {
            _context = context;
            userManager = uManager;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reviews
                .Where(a => a.IsApproved == false)
                .Include(b => b.User)
                .Include(c => c.Game)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApprovedReviews()
        {
            return View(await _context.Reviews
                .Where(a => a.IsApproved == true)
                .Include(b => b.User)
                .Include(c => c.Game)
                .ToListAsync()
                );
        }


        public IActionResult CreateReview(int GameId)
        {
            GameModel game = _context.Games.Find(GameId);
            if (game == null)
            {
                TempData["RExceptionMessage"] = "Cannot find the game you are trying to write a review for";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CreateBackUrl = GameId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([Bind("GameId, ReviewText")] ReviewModel review)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                review.UserId = user.Id;
                review.DateTime = DateTime.Now;
                review.IsApproved = false;
                if (ModelState.IsValid)
                {
                    _context.Add(review);
                    await _context.SaveChangesAsync();
                    TempData["ReviewSuccess"] = "Review has been created and sent for a evaluation before being posted";
                    return Redirect($"/Home/GameDetails/{review.GameId}");
                }
                else
                {
                    return View(review);
                }
            }
            catch (Exception x)
            {
                TempData["RException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> EditReview(int GameId)
        {
            GameModel game = _context.Games.Find(GameId);
            if (game == null)
            {
                TempData["RExceptionMessage"] = "Cannot find the game you are trying to edit the review for";
                return RedirectToAction("Index", "Home");
            }
            ApplicationUser user = await userManager.GetUserAsync(User);
            ReviewModel review = _context.Reviews.FirstOrDefault(a => a.UserId == user.Id && a.GameId == GameId);
            if (review == null)
            {
                TempData["RNotFound"] = "Cannot find the review you are trying to edit";
                return Redirect($"/Home/GameDetails/{GameId}");
            }
            ViewBag.EditBackUrl = GameId;
            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> EditReview([Bind("UserId, GameId, ReviewText")] ReviewModel review)
        {
            try
            {
                review.DateTime = DateTime.Now;
                review.IsApproved = false;
                if (ModelState.IsValid)
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                    TempData["ReviewSuccess"] = "Review has been updated, removed from posting and sent for a reevaluation before being posted.";
                    return Redirect($"/Home/GameDetails/{review.GameId}");
                }
                else
                {
                    return View(review);
                }
            }
            catch (Exception x)
            {
                TempData["RException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(ReviewModel review)
        {
            try
            {
                ReviewModel rev = await _context.Reviews
                    .FirstOrDefaultAsync(a => a.UserId == review.UserId && a.GameId == review.GameId);
                if (rev == null)
                {
                    TempData["RExceptionMessage"] = "Cannot find the review you are trying to delete";
                    return RedirectToAction("Index", "Home");
                }
                _context.Reviews.Remove(rev);
                await _context.SaveChangesAsync();
                TempData["ReviewSuccess"] = "Review has been deleted successfully.";
                return Redirect($"/Home/GameDetails/{review.GameId}");
            }
            catch (Exception x)
            {
                TempData["RException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "admin")]
        public IActionResult ReviewDetails(string UserId, int GameId)
        {
            try
            {
                ReviewModel review = _context.Reviews
                    .Include(c => c.User)
                    .Include(d => d.Game)
                    .FirstOrDefault(a => a.UserId == UserId && a.GameId == GameId);
                if (review == null)
                {
                    return NotFound();
                }
                return View(review);
            }
            catch (Exception x)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveReview(string UserId, int GameId)
        {
            ReviewModel rev = await _context.Reviews.FindAsync(UserId, GameId);
            if (rev == null)
            {
                return NotFound();
            }
            rev.IsApproved = true;
            _context.Update(rev);
            await _context.SaveChangesAsync();
            TempData["ReviewSuccess"] = "Review has been approved";
            return RedirectToAction("Index", "Review");

        }
    }
}
