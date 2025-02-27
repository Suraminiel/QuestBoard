using Microsoft.AspNetCore.Mvc;

namespace QuestBoard.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> AddTag()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListTag()
        {
            return View();
        }
    }
}
