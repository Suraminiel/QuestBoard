using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models.Domain;
using QuestBoard.Repositories;


namespace QuestBoard.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        private readonly IConfiguration configuration;
        private readonly IAppUserRepository questboardUserRepository;

        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration, IAppUserRepository questboardUserRepository) : base(options)
        {
            this.configuration = configuration;
            this.questboardUserRepository = questboardUserRepository;
        }

        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, Superadmin)
            string adminRoleId = configuration["AppSettings:adminRoleId"]; 
            string supderAdminRoleId = configuration["AppSettings:superAdminRoleId"]; 
            string useRoleId = configuration["AppSettings:useRoleId"];

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName ="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                },

                new IdentityRole
                {
                    Name= "SuperAdmin",
                    NormalizedName ="SuperAdmin",
                    Id=supderAdminRoleId,
                    ConcurrencyStamp = supderAdminRoleId,
                },

                new IdentityRole
                {
                    Name= "User",
                    NormalizedName ="User",
                    Id=useRoleId,
                    ConcurrencyStamp = useRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
           
        }

        public void SeedData(UserManager<IdentityUser> userManager)
        {
            var superAdminEmail = configuration["AppSettings:SuperAdminEmail"];
            var superAdminPassword = configuration["AppSettings:SuperAdminPassword"];

            var superAdminUser = userManager.FindByEmailAsync(superAdminEmail).Result;

            if (superAdminUser == null)
            {
                superAdminUser = new IdentityUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail
                };
                
                var result = userManager.CreateAsync(superAdminUser, superAdminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(superAdminUser, "SuperAdmin").Wait();
                    userManager.AddToRoleAsync(superAdminUser, "Admin").Wait();
                    userManager.AddToRoleAsync(superAdminUser, "User").Wait();

                    // Zugriff auf die ID des SuperAdmins
                    var superAdminId = superAdminUser.Id;

                    var AppUserProfile = new AppUser
                    {
                       Id = Guid.Parse(superAdminId),
                       Name = superAdminUser.UserName,
                    };

                    // Seed AppUser Table with SuperAdmin ID
                    questboardUserRepository.AddAsync(AppUserProfile);
                }
            }
        }
    }
}
