
namespace SolarService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }

    }
}