using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class DownloadsController : Controller
    {
        private readonly IDownloadService _downloadService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DownloadsController(IDownloadService downloadService, UserManager<ApplicationUser> uManager, IWebHostEnvironment hostEnvironment)
        {
            _downloadService = downloadService;
            userManager = uManager;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var games = await _downloadService.GetOwnedGames(user.Id);
            return View(games);
        }

        public async Task<IActionResult> DownloadGame(int gameId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                bool IsValidRequest = _downloadService.CheckOwnedGame(user.Id, gameId);
                if (IsValidRequest)
                {
                    string fileName = _downloadService.GetFileName(gameId);
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/GameFiles/", fileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                    return File(fileBytes, "text/plain", fileName);
                }
                else
                {
                    TempData["UnauthorizedDownload"] = "You cannot download a game you do not own.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception x)
            {
                TempData["DownloadException"] = "An Error has occurred. " + x.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
