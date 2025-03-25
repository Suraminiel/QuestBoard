using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace QuestBoard.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
       

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager) : base (signInManager)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
           

            return View();
        }

        public async Task<IActionResult> Privacy()
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
