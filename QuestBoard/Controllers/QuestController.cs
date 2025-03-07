using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using QuestBoard.Data;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    public class QuestController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IQuestboardTaskRepository questboardTaskRepository;
        private readonly IAppUserRepository appUserRepository;
        private readonly IProjectRepository projectRepository;

        public QuestController(ITagRepository tagRepository, IQuestboardTaskRepository questboardTaskRepository, IAppUserRepository appUserRepository, IProjectRepository projectRepository)
        {
            this.tagRepository = tagRepository;
            this.questboardTaskRepository = questboardTaskRepository;
            this.appUserRepository = appUserRepository;
            this.projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add(Guid projectId)
        {
            var tag = await tagRepository.GetAllAsync();
            var model = new AddTaskRequest
            {
                Tags = tag.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.Id.ToString()}),
                projectId = projectId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add (AddTaskRequest addTaskRequest)
        {
            var project = await projectRepository.GetAsync(addTaskRequest.projectId);

            if (project == null)
            {
                return NotFound("Projekt nicht gefunden.");
            }
            // Map viewModel to DomainModel
            var JobTask = new JobTask
            {
                Name = addTaskRequest.Name,
                Description = addTaskRequest.Description,
                Subtasks = addTaskRequest.Subtasks.Select(s => new Subtask {Name = s.Name }).ToList(),
                Type = addTaskRequest.Type,
                PublishedDate = addTaskRequest.PublishedDate,
                Author = addTaskRequest.Author,
                Priority = addTaskRequest.Priority,
                ProjectId = addTaskRequest.projectId,
                Project = project,
            };

            // Map Tags from selected Tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addTaskRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Mapping Tags back to DomainModel
            JobTask.Tags = selectedTags;

            // Map Users
            // Get Task Owner

            var Users = new List<AppUser>();
            var CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var TaskOwner = await appUserRepository.GetAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            if(TaskOwner == null)
            {
                //TaskOwner.OwnedTasks.Add(JobTask.Id);
                return View();
                
            }

            Users.Add(TaskOwner);

            JobTask.Users = Users;

            var AdminRights =  new List<Guid> ();
            AdminRights.Add(CurrentUserID);
            JobTask.AdminUserRights = AdminRights;

            //foreach(var selectedUser in )

            await questboardTaskRepository.AddAsync(JobTask);
            
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            //var TaskJobs = await questboardTaskRepository.GetAllAsync();
            var CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var TaskJobs = await questboardTaskRepository.GetAllForThisUserAsync(CurrentUserID);

            List<TaskOverview> TaskOverviews = new List<TaskOverview>();

            foreach (var Tasks in TaskJobs)
            {
                float totalSubTasks = 0; 
                float SubtasksCompleted = 0;
                float percentage = 0;

                if (Tasks.Subtasks != null)
                {
                    totalSubTasks =  Tasks.Subtasks.Count;
                    foreach (var subtask in Tasks.Subtasks)
                    {
                        if (subtask.IsCompleted)
                        {
                            SubtasksCompleted++;
                        }
                    }

                    if(totalSubTasks > 0)
                    {
                        percentage = SubtasksCompleted / totalSubTasks;
                        percentage = (float)Math.Round(percentage, 2);
                        percentage *= 100;
                    }
                    
                    
                }
                

                TaskOverviews.Add(new TaskOverview
                {
                    Id = Tasks.Id,
                    Name = Tasks.Name,
                    Description = Tasks.Description,
                    Type = Tasks.Type,
                    Tags = Tasks.Tags,
                    Priority= Tasks.Priority,
                    progress = percentage
                });
            }

/*
            foreach (var Tasks in TaskJobs)
            {
                var totalSubTasks = Tasks.Subtasks.Count;
                var SubtasksCompleted = 0;
                var percentage = 0;

                foreach(var subtask in Tasks.Subtasks)
                {
                    if(subtask.IsCompleted)
                    {
                        SubtasksCompleted++;
                    }
                }
                percentage = SubtasksCompleted / totalSubTasks;
            }

            */

            return View(TaskOverviews);
        }

        [HttpGet]
        public async Task<IActionResult>Edit(Guid Id)
        {
            var taskJob = await questboardTaskRepository.GetAsync(Id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (taskJob != null)
            {
                var model = new EditTaskRequest
                {
                    Id = taskJob.Id,
                    Name = taskJob.Name,
                    Description = taskJob.Description,
                    Subtasks = taskJob.Subtasks.Select(s => new Subtask { Name = s.Name, Id = s.Id, IsCompleted = s.IsCompleted }).ToList(),
                    Type = taskJob.Type,
                    PublishedDate = taskJob.PublishedDate,
                    Author = taskJob.Author,
                    Priority = taskJob.Priority,
                    Tags = tagsDomainModel.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = taskJob.Tags.Select(x => x.Id.ToString()).ToArray()

                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskRequest editTaskRequest)
        {
            var JobTaskDomainModel = new JobTask
            {
                Id = editTaskRequest.Id,
                Name = editTaskRequest.Name,
                Description = editTaskRequest.Description,
                Subtasks = editTaskRequest.Subtasks != null ? editTaskRequest.Subtasks.Select(s => new Subtask { Name = s.Name, Id = s.Id, IsCompleted = s.IsCompleted }).ToList() : new List<Subtask>(),
                Type = editTaskRequest.Type,
                PublishedDate = editTaskRequest.PublishedDate,
                Author = editTaskRequest.Author,
                Priority = editTaskRequest.Priority,

            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editTaskRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            JobTaskDomainModel.Tags = selectedTags;

            // Submit Info to repositiroy
            var updatedJobTask = await questboardTaskRepository.UpdateAsync(JobTaskDomainModel, editTaskRequest.DeletedSubtaskIds);
            if (updatedJobTask != null)
            {
                // show success notification
                return RedirectToAction("Edit");
            }
            // show error notification
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTaskRequest editTaskRequest)
        {
            var deletedJobTask = await questboardTaskRepository.DeleteAsync(editTaskRequest.Id);

            if (deletedJobTask != null)
            {
                // Show success notification
                return RedirectToAction("Overview");
            }

            return RedirectToAction("Edit", new { id = editTaskRequest.Id });
        }

            [HttpGet]
        public async Task<IActionResult> Calender()
        {
            return View();
        }
    }
}
