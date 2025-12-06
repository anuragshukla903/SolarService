using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolarService.Models;

namespace SolarService.Interface
{
    public interface ITenantProvider
    {
        int TenantId { get; }
        Tenant? Tenant { get; }
        void SetTenant(Tenant tenant);
    }
}