using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
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
            builder.AppendLine("Id,Title,YearReleased,RetailPrice,Category,Platform");
            foreach(var i in report)
            {
                builder.AppendLine($"{i.Id},{i.Title},{i.YearReleased},{i.RetailPrice},{i.Category},{i.Platform}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "GameListReport.csv");
        }
    }
}
