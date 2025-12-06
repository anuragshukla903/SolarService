using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarService.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }

    public int ProjectId { get; set; }
    public required Project Project { get; set; }

    public required string ItemName { get; set; }
    public int Quantity { get; set; }

    public required MaterialStatus Status { get; set; }  // Pending, InTransit, Delivered
    public required string VendorName { get; set; }
    public string? DocumentUrl { get; set; }  // Challan/GR
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int TenantId { get; set; }     // NEW
    public Tenant? Tenant { get; set; }   // optional nav
    }
}