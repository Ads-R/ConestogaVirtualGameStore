using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly GameStoreContext _context;

        public HomeController(ILogger<HomeController> logger, GameStoreContext context, UserManager<ApplicationUser> uManager)
        {
            _logger = logger;
            _context = context;
            userManager = uManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string search, string gameCategory)
        {
            var category = new SelectList(Enum.GetNames(typeof(Genre)));
            var game = from g in _context.Games select g;
            if (!String.IsNullOrEmpty(search))
            {
                //Excalamation point is a null forgiving operator for the compiler
                game = game.Where(a => a.Title!.Contains(search));
            }
            if (!String.IsNullOrEmpty(gameCategory))
            {
                game = game.Where(b => b.Category == Enum.Parse<Genre>(gameCategory));
            }
            var gameVM = new GameSearchViewModel
            {
                Categories = category,
                Games = await game.ToListAsync()
            };
            return View(gameVM);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GameDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameModel == null)
            {
                return NotFound();
            }
            var reviewsModel = await _context.Reviews
                .Include(b => b.User)
                .Where(a => a.GameId == gameModel.Id && a.IsApproved == true).ToListAsync();

            var ratingsModel = await _context.Ratings
                .Where(a => a.GameId == gameModel.Id).ToListAsync();

            GamesReviewsRatingsViewModel gamesReviewsModel = new GamesReviewsRatingsViewModel
            {
                Game = gameModel,
                Review = reviewsModel,
                ReviewCount = reviewsModel.Count,
                RatingCount = ratingsModel.Count,
                GameRatingScore = ratingsModel.Count == 0 ? 0:(double)ratingsModel.Sum(a => a.RatingScore)/ratingsModel.Count
            };

            ApplicationUser user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.HasExistingReview = _context.Reviews.Any(a => a.UserId == user.Id && a.GameId == id);
                ViewBag.HasExistingRating = _context.Ratings.Any(b => b.UserId == user.Id && b .GameId == id);
            }

            return View(gamesReviewsModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
