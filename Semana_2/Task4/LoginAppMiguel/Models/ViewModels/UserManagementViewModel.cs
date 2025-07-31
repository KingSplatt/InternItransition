namespace LoginAppMiguel.Models.ViewModels
{
    public class UserManagementViewModel
    {
        public List<UserDisplayModel> Users { get; set; } = new List<UserDisplayModel>();
        public string? Message { get; set; }
        public string? MessageType { get; set; } // success, error, warning
    }

    public class UserDisplayModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? LastLoginTime { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationTime { get; set; }
    }

    public class UserActionRequest
    {
        public List<string> UserIds { get; set; } = new List<string>();
        public string Action { get; set; } = string.Empty; // block, unblock, delete
    }
}
