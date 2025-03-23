namespace QuestBoard.Models.ViewModes
{
    public class AccountData
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string? password { get; set; }
        public string? newPassword { get; set; }
        public string? newPasswordConfirm { get; set; }
        public string profilPicturePath { get; set; }
    }
}
