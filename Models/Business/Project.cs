using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarService.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int LeadId { get; set; }
        public required Lead Lead { get; set; }

        public ProjectStatus Status { get; set; } // PendingPurchase, MaterialTransit, MaterialDelivered, Installation, Completed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }

}