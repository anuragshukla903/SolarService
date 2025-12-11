using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SolarService.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int LeadId { get; set; }
        public required Lead Lead { get; set; }

        public required string PaymentLink { get; set; }
        public required string TransactionId { get; set; }
        [Precision(18, 2)]
        public required decimal Amount { get; set; }

        public required PaymentStatus Status { get; set; }  // Pending, Success, Failed
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime? PaidAt { get; set; }
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }
}