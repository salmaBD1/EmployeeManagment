using employeeManagemement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace employeeManagemement.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            return View(currentUser);
        }

        // Requires the user to be logged in to access this action
        [Authorize]
        public IActionResult Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            return View(currentUser);
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
