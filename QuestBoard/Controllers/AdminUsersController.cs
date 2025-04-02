using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : BaseController
    {
        private readonly IUserRepository userRepository;
        private readonly IAppUserRepository appUserRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IQuestboardTaskRepository questboardTaskRepository;
        private readonly IProjectRepository projectRepository;

        public AdminUsersController(IUserRepository userRepository, IAppUserRepository appUserRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
                IQuestboardTaskRepository questboardTaskRepository, IProjectRepository projectRepository) : base(signInManager)
        {
            this.userRepository = userRepository;
            this.appUserRepository = appUserRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.questboardTaskRepository = questboardTaskRepository;
            this.projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();
            foreach (var user in users) 
            {
                bool isAdmin = false;
                if(await userManager.IsInRoleAsync(user, "Admin"))
                {
                    isAdmin = true;
                }

                usersViewModel.Users.Add(new Models.ViewModes.User
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    UserName = user.UserName,
                    Admin = isAdmin,
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                };

                var identityResult = await userManager.CreateAsync(identityUser, viewModel.Password);

                if (identityResult.Succeeded)
                {

                    // Zugriff auf die ID des SuperAdmins
                    var userId = identityUser.Id;

                    var AppUserProfile = new AppUser
                    {
                        Id = Guid.Parse(userId),
                        Name = identityUser.UserName,
                        ProfilePicturePath = "/files/images/DefaultPicxcfInvert.png"
                    };

                    // Seed AppUser Table with SuperAdmin ID
                    var appUserResult = appUserRepository.AddAsync(AppUserProfile);


                    var roles = new List<string> { "User" };
                    if(viewModel.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }
                    var roleIdentityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (roleIdentityResult.Succeeded)
                    {
                       // var signInResult = await signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
                        return RedirectToAction("List");
                    }
                }
            }
            return RedirectToAction("List"); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Delete AppUser from QuestboardDbContext
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var DeletedUser = await userManager.FindByIdAsync(id.ToString());
            // Check if currentUser is Superadmin
            if (!User.IsInRole("SuperAdmin") && await userManager.IsInRoleAsync(DeletedUser, "Admin"))
            {
                return RedirectToAction("List");
            }

            var appUser = await appUserRepository.GetAsync(id);
            if (appUser == null)
            {
                return BadRequest();
            }

            

            // Delete all Project that were created by this User
            foreach (var project in appUser.Projects)
            {
                if (project.AdminUserRights[0] == id)
                {

                    //delete tasks
                    foreach (var task in project.JobTasks)
                    {

                        var taskDeleteResult = await questboardTaskRepository.DeleteAsync(task.Id);
                        if (taskDeleteResult == null)
                        {
                            return BadRequest();
                        }

                    }
                    // delete this project
                    var projectDeletionResult = await projectRepository.DeleteAsync(project.Id);
                    if (projectDeletionResult == null)
                    {
                        return BadRequest("Failed to delete Projects of this account");
                    }
                }
            }

            var deleteResult = await appUserRepository.DeleteAsync(appUser.Id);
            if (deleteResult == null)
            {
                return BadRequest();
            }




            // Delete User from AuthDbContext

            // var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
            var identityUser = await userManager.FindByIdAsync(id.ToString());
            if (identityUser == null)
            {
                return BadRequest();
            }

            var identityResult = await userManager.DeleteAsync(identityUser);
            if (identityResult == null)
            {
                return BadRequest();
            }

            // Delete ProfilePicture
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + id);
            var profilPic = uploadPath + "\\profilPicture.png";
            if (Directory.Exists(uploadPath) && System.IO.File.Exists(profilPic))
            {
                System.IO.File.Delete(profilPic);
                Directory.Delete(uploadPath);

            }


            return RedirectToAction("List");
        }

           
    
    }
}



/*
 if(ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {

                    // Zugriff auf die ID des SuperAdmins
                    var userId = identityUser.Id;

                    var AppUserProfile = new AppUser
                    {
                        Id = Guid.Parse(userId),
                        Name = identityUser.UserName,
                        ProfilePicturePath = "/files/images/DefaultPicxcfInvert.png"
                    };

                    // Seed AppUser Table with SuperAdmin ID
                    var appUserResult = appUserRepository.AddAsync(AppUserProfile);



                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        var signInResult = await signInManager.PasswordSignInAsync(registerViewModel.Username, registerViewModel.Password, false, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
 */