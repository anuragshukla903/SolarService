using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarService.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;     // company / client name
        public string Slug { get; set; } = string.Empty;     // tenant slug e.g. "acme"
        public bool IsActive { get; set; } = true;
        // plan or connection string if DB-per-tenant
        public string? ConnectionString { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}