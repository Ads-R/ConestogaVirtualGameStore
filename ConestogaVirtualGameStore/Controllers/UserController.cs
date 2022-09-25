using Microsoft.AspNetCore.Mvc;

namespace ConestogaVirtualGameStore.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
