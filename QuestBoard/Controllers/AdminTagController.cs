using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagController : BaseController
    {
        private readonly ITagRepository tagRepository;

        public AdminTagController(ITagRepository tagRepository, SignInManager<IdentityUser> signInManager) : base (signInManager)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AddTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            await tagRepository.AddAsync(tag);

            return RedirectToAction("ListTag");
        }

        [HttpGet]
        public async Task<IActionResult> ListTag()
        {
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return View(editTagRequest);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest tagRequest)
        {
            var tag = new Tag
            {
                Id = tagRequest.Id,
                Name = tagRequest.Name,
                DisplayName = tagRequest.DisplayName,
            };
            
            var updateTag = await tagRepository.UpdateAsync(tag);

            if (updateTag != null)
            {
                // show success notification
            }
            else
            {
                // show error notification
            }

            return RedirectToAction("Edit", new {id = tagRequest.Id});
        }
        [HttpPost]
        public async Task<ActionResult> Delete(EditTagRequest tagRequest) 
        {
            var deletedTag = await tagRepository.DeleteAsync(tagRequest.Id);
            if (deletedTag != null)
            {
                // success notification
                return RedirectToAction("ListTag");
            }

            return RedirectToAction("Edit", new { id = tagRequest.Id });
        }
    }
}
