using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarService.Models
{
    public class Installation
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public required Project Project { get; set; }

        public int? AssignedToId { get; set; } // Field Boy
        public User? AssignedTo { get; set; }

        public required InstallationStage Stage { get; set; } // SiteVisit, Started, Completed

        public string? BeforeImageUrl { get; set; }
        public string? AfterImageUrl { get; set; }
        public string? CustomerSignatureUrl { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }
}