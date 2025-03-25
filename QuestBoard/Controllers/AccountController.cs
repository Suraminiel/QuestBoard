using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Security.Claims;

namespace QuestBoard.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IAppUserRepository appUserRepository;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAppUserRepository appUserRepository)
            : base(signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appUserRepository = appUserRepository;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
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
                };

                // Seed AppUser Table with SuperAdmin ID
                appUserRepository.AddAsync(AppUserProfile);

                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction("Register", "Account");
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

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> ProfilSettings()
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

            return View(accountData);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> ProfilSettings(AccountData accountData)
        {
           

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


                currentUser.Email = accountData.email;
                currentUser.UserName = accountData.name;

                if (accountData.newPassword != null &&  
                    accountData.newPasswordConfirm != null &&
                    accountData.newPassword == accountData.newPasswordConfirm)
                {
                    currentUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(currentUser, accountData.newPassword);
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
                return BadRequest("wrong pw");
            }

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

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\ProfilPictures\\" + CurrentUserId);
            var profilPic = uploadPath + "\\profilPicture.png";
            if (Directory.Exists(uploadPath) && System.IO.File.Exists(profilPic))
            {
                    System.IO.File.Delete(profilPic);
                    Directory.Delete(uploadPath);
            
            }

                return RedirectToAction("ProfilSettings");
        }

      
    }
}
