using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SolarService.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public required Project Project { get; set; }

        public required string InvoiceNumber { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public required string PdfUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }
}