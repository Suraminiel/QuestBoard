using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuestBoard.Models.Domain;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    public class BaseController : Controller
    {
        private readonly string _profileImagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "ProfilPictures");
        private readonly SignInManager<IdentityUser> signInManager;

        public BaseController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (signInManager.IsSignedIn(User))
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                var profileImagePath = Path.Combine(_profileImagePath, currentUserId, "profilPicture.png");

                if (System.IO.File.Exists(profileImagePath))
                {
                    ViewData["ProfilImage"] = $"/files/profilPic/{currentUserId}";
                }
                else
                {
                    ViewData["ProfilImage"] = $"/files/images/DefaultPicxcfInvert.png";

                }
            }
        }

        public bool hasAdminRights(Projects currentProject)
        {
            var CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!currentProject.AdminUserRights.Contains(CurrentUserID))
            {
                return false;
            }
            return true;
        }
    }
}
