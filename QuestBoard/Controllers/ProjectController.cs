using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Collections;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    public class ProjectController : Controller
    {
        private readonly IAppUserRepository appUserRepository;
        private readonly IProjectRepository projectRepository;

        public ProjectController(IAppUserRepository appUserRepository, IProjectRepository projectRepository)
        {
            this.appUserRepository = appUserRepository;
            this.projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            Guid CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            IEnumerable<Projects>? projects = await projectRepository.GetAllForThisUserAsync(CurrentUserID);
            
            List<ProjectsOverview> projectsOverviews = new List<ProjectsOverview>();

            foreach(var project in projects)
            {
                projectsOverviews.Add(new ProjectsOverview
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                });
            }

            return View(projectsOverviews);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectRequest addProjectRequest)
        {
            Projects model = new Projects
            {
                Name = addProjectRequest.Name,
                Description = addProjectRequest.Description,
            };


            // Add User with reading and or writing access
            var Users = new List<AppUser>();
            var CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var TaskOwner = await appUserRepository.GetAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            if (TaskOwner == null)
            {
                //TaskOwner.OwnedTasks.Add(JobTask.Id);
                return View();

            }

            Users.Add(TaskOwner);

            model.Users = Users;

            var AdminRights = new List<Guid>();
            AdminRights.Add(CurrentUserID);
            model.AdminUserRights = AdminRights;

            projectRepository.AddAsync(model);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return View();
        }
    }
}
