using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameStoreContext _context;

        public HomeController(ILogger<HomeController> logger, GameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var game = from g in _context.Games select g;
            if (!String.IsNullOrEmpty(search))
            {
                //Excalamation point is a null forgiving operator for the compiler
                game = game.Where(a => a.Title!.Contains(search));
            }
            return View(await game.ToListAsync());
        }

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

            return View(gameModel);
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
