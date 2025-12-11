using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarService.Models.Master
{
    public class PanelMaster
    {
        [Key]
        public int Id { get; set; }
        public int PanelId { get; set; }
        public string Brand { get; set; }
        public string? Model { get; set; }
        public int Watt { get; set; }
        public string Type { get; set; }
        public double? Efficiency { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
