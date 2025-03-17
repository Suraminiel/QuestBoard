using Microsoft.AspNetCore.Mvc;

namespace QuestBoard.Controllers
{
    public class ProjectForum : Controller
    {
        public async Task<IActionResult> List()
        {
            return View();
        }
    }
}
