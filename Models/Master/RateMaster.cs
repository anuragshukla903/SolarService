namespace SolarService.Models.Master;

public class RateMaster
{
    public int Id { get; set; }
    public string SystemType { get; set; } // Ongrid / Hybrid
    public decimal PricePerKW { get; set; }
    public decimal GstPercent { get; set; }
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}