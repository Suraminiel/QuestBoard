using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IAppUserRepository appUserRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IQuestboardTaskRepository questboardTaskRepository;
        private readonly IPasswordValidator<IdentityUser> passwordValidator;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAppUserRepository appUserRepository, IProjectRepository projectRepository, IQuestboardTaskRepository questboardTaskRepository , IPasswordValidator<IdentityUser> passwordValidator)
            : base(signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appUserRepository = appUserRepository;
            this.projectRepository = projectRepository;
            this.questboardTaskRepository = questboardTaskRepository;
            this.passwordValidator = passwordValidator;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
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
           
            return View();
        }

        

        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            { 
                ReturnUrl = ReturnUrl 
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
               if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<AccountData> createAccountDataViewModel()
        {
            //appUserRepository
            Guid CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var Username = User?.Identity?.Name;


            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + CurrentUserID);
            if (!Directory.Exists(uploadPath))
            {
                uploadPath = "/files/images/DefaultPicxcfInvert.png";
            }
            else
            {
                uploadPath = "/files/profilPic/" + CurrentUserID;
            }
            //UploadedFiles\ProfilPictures\48cdf196-f0d2-4681-90ef-1761e1caa9a3


            AccountData accountData = new AccountData
            {
                name = Username,
                email = UserEmail,
                id = CurrentUserID,
                profilPicturePath = uploadPath,
            };

            return accountData;
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> ProfilSettings()
        {
            var accountData = await createAccountDataViewModel();

            return View(accountData);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> ProfilSettings(AccountData accountData)
        {
           bool validPW = true;
           if(!ModelState.IsValid)
            {
                var model = await createAccountDataViewModel();

                return View(model);
            }

            var currentUser = await userManager.FindByIdAsync(accountData.id.ToString());
            var appUserProfil = await appUserRepository.GetAsync(Guid.Parse(currentUser.Id));

            if (currentUser == null || appUserProfil == null)
            {
                return BadRequest("Could not find User");
            }

            bool authorized = false;

            if (accountData.password != null)
            {
               authorized = await userManager.CheckPasswordAsync(currentUser, accountData.password);
            }

            
            if (authorized)
            {

                
                var currentUserName = currentUser.UserName;
                var currentEmail = currentUser.Email;

                currentUser.Email = accountData.email;
                currentUser.UserName = accountData.name;

                var usernameValidator = new UserValidator<IdentityUser>();
                var userNameResult = await usernameValidator.ValidateAsync(userManager, currentUser);
                if (!userNameResult.Succeeded)
                {
                    currentUser.UserName = currentUserName;
                }

                var emailValidator = new EmailAddressAttribute();
                if(!emailValidator.IsValid(currentUser.Email))
                {
                    currentUser.Email = currentEmail;
                }

                if (accountData.newPassword != null &&  
                    accountData.newPasswordConfirm != null &&
                    accountData.newPassword == accountData.newPasswordConfirm)
                {
                   //Validate new password
                   var validationResult = await passwordValidator.ValidateAsync(userManager, currentUser, accountData.newPassword);
                    if (validationResult.Succeeded)
                    {
                        currentUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(currentUser, accountData.newPassword);
                    }
                    else 
                    {
                        TempData["InvalidPassword"] = "The password must be at least 6 characters long and contain a number, an uppercase and lowercase letter, as well as a special character.";
                        validPW = false;
                    }
                }

                // superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin@123");


                var updatedUser = await userManager.UpdateAsync(currentUser);

                if (updatedUser == null)
                {
                    return BadRequest();
                }
                await signInManager.RefreshSignInAsync(currentUser);



                appUserProfil.Name = currentUser.UserName;

                var updatedAppUserProfil = appUserRepository.UpdateAsync(appUserProfil);

                if (updatedAppUserProfil == null)
                {
                    return BadRequest();
                }
            }
            else 
            {
                
                TempData["ErrorMessage"] = "Wrong password!";
                // return BadRequest("wrong pw");
                return RedirectToAction("ProfilSettings");
            }

            if (validPW) TempData["SuccessMessage"] = "Changes have been saved!"; else TempData["SuccessMessage"] = "Changes were saved except for the password.";
            return RedirectToAction("ProfilSettings");    
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task <IActionResult> UploadPicture (IFormFile Picture)
        {

            if (Picture != null)
            {
                try
                {
                    var type = Picture.ContentType;

                    if (type != "image/png")
                    {
                        return BadRequest("Wrong File Type");
                    }

                    var CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (CurrentUserId == null)
                    {
                        return BadRequest("");
                    }
                    var appUserProfil = await appUserRepository.GetAsync(Guid.Parse(CurrentUserId));

                    if (appUserProfil == null) 
                    {
                        return BadRequest();
                    }

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + CurrentUserId);
                    if(!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filename = "profilPicture.png";
                    var filePath = Path.Combine(uploadPath, filename);

                    if (System.IO.File.Exists(filePath))
                    {
                       // return BadRequest("file already exist");
                    }

                    using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        Picture.CopyTo(stream);
                    }

                    // Update FilePath in App Userr
                    appUserProfil.ProfilePicturePath = "/files/profilPic/" + CurrentUserId;
                    var updateAppUser = await appUserRepository.UpdateAsync(appUserProfil);

                    if (updateAppUser != null) 
                    {
                        // success
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Fehler beim Upload: " + ex.Message);
                }
            }

            return RedirectToAction("ProfilSettings");
        }

        [HttpGet]
        public async Task<IActionResult> DeletePic ()
        {

            var CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (CurrentUserId == null)
            {
                return BadRequest("");
            }

            var appUserProfil = await appUserRepository.GetAsync(Guid.Parse(CurrentUserId));

            if (appUserProfil == null)
            {
                return BadRequest();
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + CurrentUserId);
            var profilPic = uploadPath + "\\profilPicture.png";
            if (Directory.Exists(uploadPath) && System.IO.File.Exists(profilPic))
            {
                    System.IO.File.Delete(profilPic);
                    Directory.Delete(uploadPath);
            
            }

            // Update FilePath in App Userr
            appUserProfil.ProfilePicturePath = "/files/images/DefaultPicxcfInvert.png";
            var updateAppUser = await appUserRepository.UpdateAsync(appUserProfil);

            if (updateAppUser != null)
            {
                // success
            }


            return RedirectToAction("ProfilSettings");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(AccountData accountData)
        {
            if (accountData != null &&
                accountData.deleteConfirmation == "DELETE")
            {
                // Delete AppUser from QuestboardDbContext
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var appUser = await appUserRepository.GetAsync(Guid.Parse(currentUserId));
                if (appUser == null) 
                {
                    return BadRequest();
                }

                // Delete all Project that were created by this User
                foreach (var project in appUser.Projects)
                {
                    if (project.AdminUserRights[0] == Guid.Parse(currentUserId))
                    {

                        //delete tasks
                        foreach(var task in project.JobTasks)
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
                var identityUser = await userManager.FindByIdAsync(currentUserId);
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
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + currentUserId);
                var profilPic = uploadPath + "\\profilPicture.png";
                if (Directory.Exists(uploadPath) && System.IO.File.Exists(profilPic))
                {
                    System.IO.File.Delete(profilPic);
                    Directory.Delete(uploadPath);

                }


                return RedirectToAction("Logout");
            }

            return BadRequest("Failed to delete account");
        }
    }
}
