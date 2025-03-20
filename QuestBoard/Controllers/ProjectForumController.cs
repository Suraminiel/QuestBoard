using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    public class ProjectForumController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IAppUserRepository appUserRepository;
        private readonly IForumPostRepository forumPostRepository;
        private readonly IForumThreadRepository forumThreadRepository;

        public ProjectForumController(IProjectRepository projectRepository, IAppUserRepository appUserRepository, 
            IForumPostRepository forumPostRepository, IForumThreadRepository forumThreadRepository)
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
           
            if (currentProject == null || ForumThreadsModel == null)
            {
                return NotFound();
            }

            var ForumThreads = new List<ForumThreadsViewModel>();

            ForumThreadsContainerViewModel forumThreadsContainerViewModel = new ForumThreadsContainerViewModel
            {
                ProjectId = currentProject.Id,
                ForumThreads = ForumThreads,

            };

            // Add loop through existing threads. map them to ForumThreadViewModel and add them to forumThreadsContainerViewModel
            foreach (var forumThread in ForumThreadsModel)
            {
                forumThreadsContainerViewModel.ForumThreads.Add(new ForumThreadsViewModel
                {
                    id = forumThread.id,
                    name = forumThread.name,
                    Postings = forumThread.Postings,
                    Project = forumThread.Project,
                    ProjectId = forumThread.ProjectId,
                    author = forumThread.Postings[0].User.Name,
                });
            }

            return View(forumThreadsContainerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(ForumThreadsContainerViewModel forumThreadsContainerViewModel)
        {
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
            return RedirectToAction("ShowThread", new { ProjectId = forumPostViewModel.ProjectId, ThreadId =forumPostViewModel.ThreadId });
        }

    }
}
