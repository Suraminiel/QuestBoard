namespace QuestBoard.Models.ViewModes
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool Admin { get; set; }
    }
}
