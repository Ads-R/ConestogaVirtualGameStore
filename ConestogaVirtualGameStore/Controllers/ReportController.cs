using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        private readonly GameStoreContext _context;
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService, GameStoreContext context)
        {
            _reportService = reportService;
            _context = context;
        }
        public IActionResult Index(string reportType)
        {
            if (reportType == "" || reportType == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(reportType);
            }
        }

        public async Task<IActionResult> GameList()
        {
            var report = await _reportService.GetAllGames();
            return View(report);
        }

        public async Task<IActionResult> DownloadGameListReport()
        {
            var report = await _reportService.GetAllGames();
            var builder = new StringBuilder();
            builder.AppendLine("Title,YearReleased,RetailPrice,Category,Platform");
            foreach (var i in report)
            {
                builder.AppendLine($"{i.Title},{i.YearReleased},{i.RetailPrice},{i.Category},{i.Platform}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "GameListReport.csv");
        }
        public async Task<IActionResult> GameDetail()
        {
            var report = await _reportService.GetAllGames();
            return View(report);
        }
        public async Task<IActionResult> DownloadGameDetailReport()
        {
            var report = await _reportService.GetAllGames();
            var builder = new StringBuilder();
            builder.AppendLine("Title,YearReleased,RetailPrice,Category,Platform,Description");
            foreach (var i in report)
            {
                builder.AppendLine($"{i.Title},{i.YearReleased},{i.RetailPrice},{i.Category},{i.Platform},{i.Description}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "GameDetailReport.csv");
        }
        public async Task<IActionResult> MemberList()
        {
            var report = await _reportService.GellAllUsers();
            return View(report);
        }
        public async Task<IActionResult> DownloadMemberListReport()
        {
            var report = await _reportService.GellAllUsers();
            var builder = new StringBuilder();
            builder.AppendLine("UserName,Member Name,Favourate Category,Favourate Platform");
            foreach (var i in report)
            {
                builder.AppendLine($"{i.UserName},{i.Profile.FirstName} {i.Profile.LastName},{i.Preference.Category},{i.Preference.Platform}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "MemberListReport.csv");
        }
        public async Task<IActionResult> MemberDetail()
        {
            var report = await _reportService.GellAllUsers();
            return View(report);
        }
        public async Task<IActionResult> DownloadMemberDetailReport()
        {
            var report = await _reportService.GellAllUsers();
            var builder = new StringBuilder();
            builder.AppendLine("UserName,Member Name,Gender,Email,Mailing Address,Shipping Address,Favourate Category,Favourate Platform");
            foreach (var i in report)
            {
                builder.AppendLine($"{i.UserName},{i.Profile.FirstName} {i.Profile.LastName},{i.Profile.Gender},{i.Email},{i.Address.MailingAddress},{i.Address.ShippingAddress},{i.Preference.Category},{i.Preference.Platform}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "MemberDetailReport.csv");
        }


        public async Task<IActionResult> WishList()
        {
            var report = await _reportService.GetAllWishes();
            var gameList = (await _reportService.GetAllGames()).Select(e => e.Id);
            List<WishGameCount> games = new List<WishGameCount>();
            foreach (var i in gameList)
            {
                int wishCount = report.Where(b => b.GameId == i).Count();
                string gameTitle = _context.Games.Where(c => c.Id == i).Select(d => d.Title).FirstOrDefault();
                WishGameCount wishGame = new WishGameCount()
                {
                    GameTitle = gameTitle,
                    Count = wishCount
                };
                games.Add(wishGame);
            }
            return View(games);
        }
        public async Task<IActionResult> DownloadWishListReport()
        {
            var report = await _reportService.GetAllWishes();
            var gameList = (await _reportService.GetAllGames()).Select(e => e.Id);
            List<WishGameCount> games = new List<WishGameCount>();
            foreach (var i in gameList)
            {
                int wishCount = report.Where(b => b.GameId == i).Count();
                string gameTitle = _context.Games.Where(c => c.Id == i).Select(d => d.Title).FirstOrDefault();
                WishGameCount wishGame = new WishGameCount()
                {
                    GameTitle = gameTitle,
                    Count = wishCount
                };
                games.Add(wishGame);
            }
            var builder = new StringBuilder();
            builder.AppendLine("Game Name,Number of Times Added to wish list");
            foreach (var i in games)
            {
                builder.AppendLine($"{i.GameTitle},{i.Count}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "WishListReport.csv");
        }

       
        public async Task<IActionResult> SalesList()
        {
            var report = await _reportService.GetAllOrders();
            List<Sales> gameSales = new List<Sales>();
            int months = 12;
            for (int i = 1; i < months + 1; i++)
            {
                var quantity = report.Where(a => a.DateOfPurchase.Month == i).Count();
                var total = report.Where(a => a.DateOfPurchase.Month == i).Select(b => b.Price).Sum();
                var m = "";
                var grandTotalQuantities = +quantity;
                var grandTotalSales = +total;
                if (i == 1)
                {
                    m = "January";
                }
                else if (i == 2)
                {
                    m = "Feburary";
                }
                else if (i == 3)
                {
                    m = "March";
                }
                else if (i == 4)
                {
                    m = "April";
                }
                else if (i == 5)
                {
                    m = "May";
                }
                else if (i == 6)
                {
                    m = "June";
                }
                else if (i == 7)
                {
                    m = "July";
                }
                else if (i == 8)
                {
                    m = "August";
                }
                else if (i == 9)
                {
                    m = "September";
                }
                else if (i == 10)
                {
                    m = "October";
                }
                else if (i == 11)
                {
                    m = "November";
                }
                else
                {
                    m = "December";
                }
                Sales sales = new Sales()
                {
                    Month = m,
                    Quantity = quantity,
                    TotalSales = total,
                    GrandTotalQuantities = grandTotalQuantities,
                    GrandTotalSales = grandTotalSales,
                };
                gameSales.Add(sales);
            }
            return View(gameSales);
        }
        public async Task<IActionResult> DownloadSalesListReport()
        {
            var report = await _reportService.GetAllOrders();
            List<Sales> gameSales = new List<Sales>();
            int months = 12;
            for (int i = 1; i < months + 1; i++)
            {
                var quantity = report.Where(a => a.DateOfPurchase.Month == i).Count();
                var total = report.Where(a => a.DateOfPurchase.Month == i).Select(b => b.Price).Sum();
                var m = "";
                var grandTotalQuantities = +quantity;
                var grandTotalSales = +total;
                if (i == 1)
                {
                    m = "January";
                }
                else if (i == 2)
                {
                    m = "Feburary";
                }
                else if (i == 3)
                {
                    m = "March";
                }
                else if (i == 4)
                {
                    m = "April";
                }
                else if (i == 5)
                {
                    m = "May";
                }
                else if (i == 6)
                {
                    m = "June";
                }
                else if (i == 7)
                {
                    m = "July";
                }
                else if (i == 8)
                {
                    m = "August";
                }
                else if (i == 9)
                {
                    m = "September";
                }
                else if (i == 10)
                {
                    m = "October";
                }
                else if (i == 11)
                {
                    m = "November";
                }
                else
                {
                    m = "December";
                }
                Sales sales = new Sales()
                {
                    Month = m,
                    Quantity = quantity,
                    TotalSales = total,
                    GrandTotalQuantities = grandTotalQuantities,
                    GrandTotalSales = grandTotalSales,
                };
                gameSales.Add(sales);
            }
            var builder = new StringBuilder();
            builder.AppendLine("Month,Quantity,Total Sales");
            foreach (var i in gameSales)
            {
                builder.AppendLine($"{i.Month},{i.Quantity},{i.TotalSales}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "SalesListReport.csv");
        }
    }
    
}
