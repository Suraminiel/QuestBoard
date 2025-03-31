using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models.ViewModes
{
    public class AccountData
    {
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string? newPassword { get; set; }
        public string? newPasswordConfirm { get; set; }
        public string? profilPicturePath { get; set; }
        public string? deleteConfirmation { get; set; }
    }
}
