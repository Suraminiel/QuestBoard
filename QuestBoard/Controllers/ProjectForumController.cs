using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    public class ProjectForumController : BaseController
    {
        private readonly IProjectRepository projectRepository;
        private readonly IAppUserRepository appUserRepository;
        private readonly IForumPostRepository forumPostRepository;
        private readonly IForumThreadRepository forumThreadRepository;

        public ProjectForumController(IProjectRepository projectRepository, IAppUserRepository appUserRepository, 
            IForumPostRepository forumPostRepository, IForumThreadRepository forumThreadRepository, SignInManager<IdentityUser> signInManager)
            : base(signInManager) 
        {
            this.projectRepository = projectRepository;
            this.appUserRepository = appUserRepository;
            this.forumPostRepository = forumPostRepository;
            this.forumThreadRepository = forumThreadRepository;
        }
        [HttpGet]
        public async Task<IActionResult> List(Guid ProjectId)
        {
            var currentProject = await projectRepository.GetAsync(ProjectId);
            var ForumThreadsModel = await forumThreadRepository.GetAllAsyncForThisProject(ProjectId);
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentProject == null || ForumThreadsModel == null)
            {
                return NotFound();
            }

            var ForumThreads = new List<ForumThreadsViewModel>();

            ForumThreadsContainerViewModel forumThreadsContainerViewModel = new ForumThreadsContainerViewModel
            {
                ProjectId = currentProject.Id,
                Project = currentProject,
                ForumThreads = ForumThreads,

            };

            // Add loop through existing threads. map them to ForumThreadViewModel and add them to forumThreadsContainerViewModel
            foreach (var forumThread in ForumThreadsModel)
            {
                var  ThreadCreatorId = forumThread.Postings[0].UserId;
                var appUser = await appUserRepository.GetAsync(ThreadCreatorId);

                if (appUser == null)
                {
                    break;
                }
                
                forumThreadsContainerViewModel.ForumThreads.Add(new ForumThreadsViewModel
                {
                    id = forumThread.id,
                    created = forumThread.created,
                    name = forumThread.name,
                    Postings = forumThread.Postings,
                    Project = forumThread.Project,
                    ProjectId = forumThread.ProjectId,
                    author = forumThread.Postings[0].User.Name,
                    authorProfilePicturePath = appUser.ProfilePicturePath,
                    isAuthorizedToEdit = (currentProject.AdminUserRights.Contains(Guid.Parse(UserId)) ? true : false)
                });
            }

            return View(forumThreadsContainerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(ForumThreadsContainerViewModel forumThreadsContainerViewModel)
        {
            if (forumThreadsContainerViewModel.newMessage.name.IsNullOrEmpty() || forumThreadsContainerViewModel.newMessage.message.IsNullOrEmpty()) 
            {
                return RedirectToAction("List", new { ProjectId = forumThreadsContainerViewModel.ProjectId });
            }

            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var AppUser = await appUserRepository.GetAsync(Guid.Parse(UserId));

            if (AppUser == null || forumThreadsContainerViewModel == null)
            {
                return BadRequest();
            }

            // add new thread to project
            var newThread = new ForumThread
            {
                id = new Guid(),
                created = DateTime.Now,
                name = forumThreadsContainerViewModel.newMessage.name,
                ProjectId = forumThreadsContainerViewModel.ProjectId,
            };

            var added = await forumThreadRepository.AddAsync(newThread);

            var currentThread = await forumThreadRepository.GetAsync(newThread.id);

            if(added == null || currentThread == null)
            {
                return BadRequest();
            }

            // add new message to this thread
            var newMessage = new ForumPost
            {
                name = "initial Post",
                created = DateTime.Now,
                message = forumThreadsContainerViewModel.newMessage.message,
                Thread = currentThread,
                ThreadId = currentThread.id,
                User = AppUser,
                UserId = AppUser.Id
            };

            var posted = await forumPostRepository.AddAsync(newMessage);

            if(posted == null )
            {
                return BadRequest();
            }

            return RedirectToAction("List", new { ProjectId = forumThreadsContainerViewModel.ProjectId });
        }

        [HttpGet]
        public async Task<IActionResult> ShowThread(Guid ProjectId, Guid ThreadId)
        {
           var currentProject = await projectRepository.GetAsync(ProjectId);
           var thread = await forumThreadRepository.GetAsync(ThreadId);
            
            if(thread == null || currentProject == null)
            {
                return BadRequest();
            }

            
            var threadPostingsModel = new List<ForumPostViewModel>();

            foreach(var post in thread.Postings)
            {
                threadPostingsModel.Add(new ForumPostViewModel
                {
                    id  = post.id,
                    created = post.created,
                    name = post.name,
                    message = post.message,
                    ThreadName = thread.name,
                    ProjectId = currentProject.Id,
                    User = post.User,
                    UserId = post.UserId,
                    Thread = post.Thread,
                    ThreadId = ThreadId,
                });
            }
         
            return View(threadPostingsModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(ForumPostViewModel forumPostViewModel)
        {
            if (forumPostViewModel.newMessage.IsNullOrEmpty())
            {
                return RedirectToAction("ShowThread", new { ProjectId = forumPostViewModel.ProjectId, ThreadId = forumPostViewModel.ThreadId });
            }

            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var AppUser = await appUserRepository.GetAsync(Guid.Parse(UserId));

            if (AppUser == null || forumPostViewModel == null)
            {
                return BadRequest();
            }

            // add new message to this thread
            var newMessage = new ForumPost
            {
                name = "initial Post",
                created = DateTime.Now,
                message = forumPostViewModel.newMessage,
              //  Thread = currentThread,
                ThreadId = forumPostViewModel.ThreadId,
                User = AppUser,
                UserId = AppUser.Id
            };

            var posted = await forumPostRepository.AddAsync(newMessage);

            if (posted == null)
            {
                return BadRequest();
            }

            return RedirectToAction("ShowThread", new { ProjectId = forumPostViewModel.ProjectId, ThreadId =forumPostViewModel.ThreadId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteThread(Guid threadId, Guid ProjectId)
        {
            var deleted = await forumThreadRepository.DeleteAsync(threadId);
            if (deleted == null)
            {
                return BadRequest();
            }

            return RedirectToAction("List", new { ProjectId = ProjectId });
        }
    }
}
