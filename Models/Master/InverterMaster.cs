namespace SolarService.Models.Master;

public class InverterMaster
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal CapacityKW { get; set; }
    public string Type { get; set; }
    public int WarrantyYears { get; set; }
    public bool IsActive { get; set; } = true;
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}