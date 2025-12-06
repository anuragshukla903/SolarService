using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SolarService.Models
{
    public class Quotation
    {
        public int Id { get; set; }

        public int LeadId { get; set; }
        public required Lead Lead { get; set; }

        public required string PdfUrl { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        public bool IsSentOnWhatsapp { get; set; }
        public DateTime SentAt { get; set; }

        public QuotationStatus Status { get; set; } // Accepted/Rejected/Bargain/Pending
        public string? CustomerRemark { get; set; }
        public int TenantId { get; set; }     // NEW
        public Tenant? Tenant { get; set; }   // optional nav
    }
}