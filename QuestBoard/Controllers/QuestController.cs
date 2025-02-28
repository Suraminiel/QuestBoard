using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;

namespace QuestBoard.Controllers
{
    public class QuestController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IQuestboardTaskRepository questboardTaskRepository;

        public QuestController(ITagRepository tagRepository, IQuestboardTaskRepository questboardTaskRepository)
        {
            this.tagRepository = tagRepository;
            this.questboardTaskRepository = questboardTaskRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tag = await tagRepository.GetAllAsync();
            var model = new AddTaskRequest
            {
                Tags = tag.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.Id.ToString()})
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add (AddTaskRequest addTaskRequest)
        {
            // Map viewModel to DomainModel
            var JobTask = new JobTask
            {
                Name = addTaskRequest.Name,
                Description = addTaskRequest.Description,
                Type = addTaskRequest.Type,
                PublishedDate = addTaskRequest.PublishedDate,
                Author = addTaskRequest.Author,
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
            await questboardTaskRepository.AddAsync(JobTask);
            
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            var TaskJobs = await questboardTaskRepository.GetAllAsync();
            return View(TaskJobs);
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
                    Type = taskJob.Type,
                    PublishedDate = taskJob.PublishedDate,
                    Author = taskJob.Author,
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
                Type = editTaskRequest.Type,
                PublishedDate = editTaskRequest.PublishedDate,
                Author = editTaskRequest.Author,

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
            var updatedJobTask = await questboardTaskRepository.UpdateAsync(JobTaskDomainModel);
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
