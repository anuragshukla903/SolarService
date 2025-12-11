namespace SolarService.Models.Master;

public class StructureMaster
{
    public int Id { get; set; }
    public string StructureType { get; set; }
    public string Material { get; set; }
    public decimal CostPerKW { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}