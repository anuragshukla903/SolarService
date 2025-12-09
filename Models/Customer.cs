
namespace SolarService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
        public string? AlternateMobile { get; set; }

        public required string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}