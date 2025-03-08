using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class EditProjectRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<TaskOverview> TaskOverviews { get; set; }
        public List<Guid> AdminUserRights { get; set; }
        public ICollection<JobTask> JobTasks { get; set; }

        public ICollection<AppUserViewModel> Users { get; set; }
        public string InvitedUser { get; set; }
    }
}
