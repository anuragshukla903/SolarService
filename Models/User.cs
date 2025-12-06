namespace SolarService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }   // FIXED
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDelete { get; set; }
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }
    public class Role
    {
        public int Id { get; set; }  // 1: SuperAdmin, 2: Manager, 3: TeamLead, 4: FieldBoy
        public string? Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

    }
}