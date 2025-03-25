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
        private readonly string _profileImagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "ProfilPictures");

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager) : base (signInManager)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
           /*
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profileImagePath = Path.Combine(_profileImagePath, currentUserId, "profilPicture.png");

            if (System.IO.File.Exists(profileImagePath))
            {
                ViewData["ProfilImage"] = $"/files/profilPic/{currentUserId}";
            }
            else
            {
                ViewData["ProfilImage"] = $"/files/images/DefaultPicxcfInvert.png";
                //  <img class="round-img-small" src="/files/images/DefaultPicxcfInvert.png" />
            }*/

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
