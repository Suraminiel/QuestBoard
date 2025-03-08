using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    public class ProjectController : Controller
    {
        private readonly IAppUserRepository appUserRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IQuestboardTaskRepository questboardTaskRepository;

        public ProjectController(IAppUserRepository appUserRepository, IProjectRepository projectRepository, IQuestboardTaskRepository questboardTaskRepository)
        {
            this.appUserRepository = appUserRepository;
            this.projectRepository = projectRepository;
            this.questboardTaskRepository = questboardTaskRepository;
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
            var currentProject = await projectRepository.GetAsync(id);
            
            if (currentProject != null)
            {
                var editProject = new EditProjectRequest
                {
                    Name = currentProject.Name,
                    Description = currentProject.Description,
                    Id = currentProject.Id,
                    //Users = currentProject.Users,
                    AdminUserRights = currentProject.AdminUserRights.ToList(),
                    //Subtasks = taskJob.Subtasks.Select(s => new Subtask { Name = s.Name, Id = s.Id, IsCompleted = s.IsCompleted }).ToList(),
                    TaskOverviews = currentProject.JobTasks.Select(t  => new TaskOverview { 
                        Priority = t.Priority,
                        Name = t.Name,
                        Id = t.Id, 
                        //AdminUserRights = t.AdminUserRights,
                        Author = t.Author,
                        Description = t.Description,
                       // ProjectId = t.ProjectId,
                        Type = t.Type,
                        Tags = t.Tags,
                        Subtasks = t.Subtasks}).ToList(),

                  
                };

                ICollection<AppUserViewModel> Users = new List<AppUserViewModel>();
                foreach (var users in currentProject.Users)
                {
                    AppUserViewModel usermodel = new AppUserViewModel
                    {
                        Id = users.Id,
                        Name = users.Name,

                        

                    };

                    
                    if(currentProject.AdminUserRights.Contains(users.Id))
                    {
                        usermodel.IsAdminOfSelectedProject = true;
                    }
                    else
                    {
                        usermodel.IsAdminOfSelectedProject = false;
                    }

                    

                    Users.Add(usermodel);
                    
                }

                editProject.Users = Users.ToList();

                foreach (var task in editProject.TaskOverviews)
                {
                    float totalSubTasks = 0;
                    float SubtasksCompleted = 0;
                    float percentage = 0;

                    if (task.Subtasks != null)
                    {
                        totalSubTasks = task.Subtasks.Count;
                        foreach (var subtask in task.Subtasks)
                        {
                            if (subtask.IsCompleted)
                            {
                                SubtasksCompleted++;
                            }
                        }

                        if (totalSubTasks > 0)
                        {
                            percentage = SubtasksCompleted / totalSubTasks;
                            percentage = (float)Math.Round(percentage, 2);
                            percentage *= 100;
                        }
                    }

                    task.progress = percentage;
                }

               


                return View(editProject);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectRequest editProjectRequest)
        {

            // get current project from database
            var currentProject = await projectRepository.GetAsync(editProjectRequest.Id);
           
            // loop through editprojectusers and add Id with adminrights to currenproject.AdminUserRights
            foreach (var users in editProjectRequest.Users)
            {
                if(users.IsAdminOfSelectedProject)
                {
                    if(!currentProject.AdminUserRights.Contains(users.Id))
                    {
                        currentProject.AdminUserRights.Add(users.Id);
                    }
                }
                else
                {
                    if (currentProject.AdminUserRights.Contains(users.Id))
                    {
                        currentProject.AdminUserRights.Remove(users.Id);
                    }
                }
            }

            // save current project to database
            var saveCurrentProject = await projectRepository.UpdateAsync(currentProject);

            if (saveCurrentProject != null)
            {
                return RedirectToAction("Edit", new { id = editProjectRequest.Id });
            }
             return new ContentResult
            {
                Content = "Something went wrong. Please try again later.",
                ContentType = "text/plain",
                StatusCode = 500
            }; ;
        }

        public async Task<IActionResult> Delete(EditProjectRequest editProjectRequest)
        {
            bool successfullDeletion = true;
            var currentProject = await projectRepository.GetAsync(editProjectRequest.Id);

            if (currentProject != null)
            {
                var editProject = new EditProjectRequest
                {
                    Name = currentProject.Name,
                    Description = currentProject.Description,
                    Id = currentProject.Id,

                    //Subtasks = taskJob.Subtasks.Select(s => new Subtask { Name = s.Name, Id = s.Id, IsCompleted = s.IsCompleted }).ToList(),
                    JobTasks = currentProject.JobTasks.Select(t => new JobTask
                    {
                        Priority = t.Priority,
                        Name = t.Name,
                        Id = t.Id,
                        //AdminUserRights = t.AdminUserRights,
                        Author = t.Author,
                        Description = t.Description,
                        // ProjectId = t.ProjectId,
                        Type = t.Type,
                        Tags = t.Tags,
                        Subtasks = t.Subtasks
                    }).ToList(),


                };

               

                foreach (var task in editProject.JobTasks)
                {

                   var deleteTask = await questboardTaskRepository.DeleteAsync(task.Id);
                    if (deleteTask == null)
                    {
                        successfullDeletion = false;
                    }
                }
            }

            var deletedProject = await projectRepository.DeleteAsync(editProjectRequest.Id);

            if (deletedProject != null && successfullDeletion)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editProjectRequest.Id });
            /* var deletedJobTask = await questboardTaskRepository.DeleteAsync(editTaskRequest.Id);

            if (deletedJobTask != null)
            {
                // Show success notification
                return RedirectToAction("Overview");
            }*/


        }

        [HttpPost]
        public async Task<IActionResult> inviteTeamMember(EditProjectRequest editProjectRequest)
        {
            // get current project
            var currentProject = await projectRepository.GetAsync(editProjectRequest.Id);

            // look for invited User in appUserRepository
            var newUser = await appUserRepository.GetAsync(editProjectRequest.InvitedUser);

            // if possible add invited User to project
            if (newUser != null && currentProject != null)
            {
                currentProject.Users.Add(newUser);
               var updatedProject = await projectRepository.UpdateAsync(currentProject);

                if (updatedProject != null)
                {
                    // insert victory dance
                }
            }

            

            return RedirectToAction("Edit", new { id = editProjectRequest.Id });
        }
    }
}
