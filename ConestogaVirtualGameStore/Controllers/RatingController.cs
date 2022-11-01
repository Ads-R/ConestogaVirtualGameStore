using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GameStoreContext _context;
        public RatingController(GameStoreContext context, UserManager<ApplicationUser> uManager)
        {
            _context = context;
            userManager = uManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RateGame(int GameId)
        {
            GameModel game = _context.Games.Find(GameId);
            if (game == null)
            {
                TempData["RateExceptionMessage"] = "Cannot find the game you are trying to give a rating score";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.RateCreateBackUrl = GameId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RateGame([Bind("GameId, RatingScore")] RatingViewModel rate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await userManager.GetUserAsync(User);
                    RatingModel rating = new RatingModel
                    {
                        UserId = user.Id,
                        GameId = rate.GameId,
                        RatingScore = int.Parse(rate.RatingScore)
                    };
                    _context.Add(rating);
                    await _context.SaveChangesAsync();
                    TempData["RateSuccess"] = "Rating Score has been submitted successfully";
                    return Redirect($"/Home/GameDetails/{rating.GameId}");
                }
                else
                {
                    ViewBag.RateCreateBackUrl = rate.GameId;
                    return View(rate);
                }
            }
            catch (Exception x)
            {
                TempData["RateException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ChangeRating(int GameId)
        {
            GameModel game = _context.Games.Find(GameId);
            if (game == null)
            {
                TempData["RateExceptionMessage"] = "Cannot find the game you are trying to give a rating score";
                return RedirectToAction("Index", "Home");
            }
            ApplicationUser user = await userManager.GetUserAsync(User);
            RatingModel rating = _context.Ratings.FirstOrDefault(a => a.UserId == user.Id && a.GameId == GameId);
            if (rating == null)
            {
                TempData["RNotFound"] = "Cannot find the review you are trying to edit";
                return Redirect($"/Home/GameDetails/{GameId}");
            }
            RatingViewModel rate = new RatingViewModel
            {
                UserId = rating.UserId,
                GameId = rating.GameId,
                RatingScore = rating.RatingScore.ToString()
            };
            ViewBag.RateEditBackUrl = GameId;
            return View(rate);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRating([Bind("UserId, GameId, RatingScore")] RatingViewModel rate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RatingModel rating = new RatingModel
                    {
                        UserId = rate.UserId,
                        GameId = rate.GameId,
                        RatingScore = int.Parse(rate.RatingScore)
                    };
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                    TempData["RateSuccess"] = "Rating Score has been changed successfully";
                    return Redirect($"/Home/GameDetails/{rating.GameId}");
                }
                else
                {
                    ViewBag.RateEditBackUrl = rate.GameId;
                    return View(rate);
                }
            }
            catch (Exception x)
            {
                TempData["RateException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
