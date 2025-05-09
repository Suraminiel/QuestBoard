﻿using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class AppUserRepository: IAppUserRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public AppUserRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }

        public async Task<AppUser> AddAsync(AppUser appUser)
        {
            await questboardDbContext.AddAsync(appUser);
            await questboardDbContext.SaveChangesAsync();
            return appUser;
        }

        public async Task<AppUser?> DeleteAsync(Guid id)
        {
            var Appuser = await questboardDbContext.Users.FindAsync(id);
            if (Appuser != null) 
            {
                questboardDbContext.Users.Remove(Appuser);
                await questboardDbContext.SaveChangesAsync();
                return Appuser;
            }
            return null;
        }

        public Task<IEnumerable<AppUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser?> GetAsync(Guid id)
        {
            return await questboardDbContext.Users
                .Include(x => x.Projects)
                .ThenInclude(s => s.JobTasks)
                .ThenInclude(s => s.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == id);
            //return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).Include(st => st.Subtasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser?> GetAsync(string name)
        {
            return await questboardDbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<AppUser?> UpdateAsync(AppUser appUser)
        {
            var currentUser = await questboardDbContext.Users.FindAsync(appUser.Id);

            if(currentUser != null)
            {
                currentUser.Name = appUser.Name;
                currentUser.ProfilePicturePath = appUser.ProfilePicturePath;
                await questboardDbContext.SaveChangesAsync();
                return currentUser;
            }
            return null;
        }
    }
}
