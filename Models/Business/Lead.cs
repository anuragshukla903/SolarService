using System.ComponentModel.DataAnnotations.Schema;
using SolarService.Models;

public class Lead
{
    public int Id { get; set; }
    public required string LeadId { get; set; } //LD2023XXXX
    public required string CustomerName { get; set; }
    public required string Mobile { get; set; }
    public string? Email { get; set; }
    public required string Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string? LeadSource { get; set; }  // Facebook, Insta, Website, Manual
    public double RequiredKW { get; set; }

    public int? AssignedToId { get; set; }  // Team Leader / Manager / Field Boy
    public User? AssignedTo { get; set; }

    public LeadStatus Status { get; set; }  // New, QuotationSent, Accepted, Rejected, Bargain

    public string? CustomerRemark { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int TenantId { get; set; }     // NEW
    public Tenant? Tenant { get; set; }   // optional nav
}
