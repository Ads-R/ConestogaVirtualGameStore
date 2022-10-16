using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ConestogaVirtualGameStore.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class GameController : Controller
    {
        private readonly GameStoreContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GameController(GameStoreContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }

            var gameModel = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameModel == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }

            return View(gameModel);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YearReleased," +
            "RetailPrice,Description,Category,Platform,ImageFile")] GameModel gameModel)
        {
            try
            {
                if (gameModel.ImageFile == null)
                {
                    ModelState.AddModelError("ImageFile", "Cover Image is required when adding a new game");
                }
                bool IsValidDate = IsDateValid(gameModel.YearReleased);
                if (!IsDateValid(gameModel.YearReleased))
                {
                    ModelState.AddModelError("YearReleased", "Date cannot be in the future");
                }
                if (ModelState.IsValid)
                {
                    string fileName = GenerateUniqueGameName(gameModel.ImageFile);
                    gameModel.ImageName = fileName;
                    string path = GetPath(fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await gameModel.ImageFile.CopyToAsync(fileStream);
                    }
                    _context.Add(gameModel);
                    await _context.SaveChangesAsync();
                    TempData["GSuccess"] = "Game added successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception x)
            {
                TempData["GException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }

            var gameModel = await _context.Games.FindAsync(id);
            if (gameModel == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,YearReleased,RetailPrice," +
            "Description,ImageName,Category,Platform,ImageFile")] GameModel gameModel)
        {
            try
            {
                if (id != gameModel.Id)
                {
                    TempData["GNotFound"] = "Cannot find game";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    if (gameModel.ImageFile != null)
                    {
                        string oldFileName = gameModel.ImageName;
                        string fileName = GenerateUniqueGameName(gameModel.ImageFile);
                        gameModel.ImageName = fileName;
                        string path = GetPath(fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await gameModel.ImageFile.CopyToAsync(fileStream);
                        }
                        var imageLocation = GetPath(oldFileName);
                        DeleteFile(imageLocation);
                    }
                    _context.Update(gameModel);
                    await _context.SaveChangesAsync();
                    TempData["GSuccess"] = "Game modified successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception x)
            {
                TempData["GException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }

            var gameModel = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameModel == null)
            {
                TempData["GNotFound"] = "Cannot find game";
                return RedirectToAction(nameof(Index));
            }

            return View(gameModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var gameModel = await _context.Games.FindAsync(id);

                if (gameModel != null)
                {
                    var imageLocation = GetPath(gameModel.ImageName);
                    DeleteFile(imageLocation);
                    _context.Games.Remove(gameModel);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception x)
            {
                TempData["GException"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool GameModelExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }

        public string GenerateUniqueGameName(IFormFile gameImage)
        {
            string rootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(gameImage.FileName);
            string fileExtension = Path.GetExtension(gameImage.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;

            return fileName;
        }

        public string GetPath(string fileName)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath + "/Images/GameCovers/", fileName);
            return path;
        }
        public void DeleteFile(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
        public bool IsDateValid(int dob)
        {
            if (dob > DateTime.Now.Year)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
