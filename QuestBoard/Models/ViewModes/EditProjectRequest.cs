using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class EditProjectRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string shortDescription { get; set; }
        public string Description { get; set; }

        public List<TaskOverview> TaskOverviews { get; set; }
        public List<Guid> AdminUserRights { get; set; }
        public ICollection<JobTask> JobTasks { get; set; }

        public List<AppUserViewModel> Users { get; set; }

        public bool isAdmin { get; set; } = false; // Teilt View mit, ob der aktuelle User ein Projektadmin ist
        public string InvitedUser { get; set; }
    }
}
