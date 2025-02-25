using Microsoft.AspNetCore.Mvc;

namespace QuestBoard.Controllers
{
    public class QuestController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Calender()
        {
            return View();
        }
    }
}
