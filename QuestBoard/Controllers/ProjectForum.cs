using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;

namespace QuestBoard.Controllers
{
    public class ProjectForum : Controller
    {
        private readonly IProjectRepository projectRepository;

        public ProjectForum(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<IActionResult> List(Guid ProjectId)
        {
            var currentProject = await projectRepository.GetAsync(ProjectId);

            if (currentProject == null)
            {
                return NotFound();
            }

            ForumThreadsContainerViewModel forumThreadsContainerViewModel = new ForumThreadsContainerViewModel
            {
                ProjectId = currentProject.Id,
            };

            // Add loop through existing threads. map them to ForumThreadViewModel and add them to forumThreadsContainerViewModel

            return View(forumThreadsContainerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddThread()
        {
            return View();
        }
    }
}
