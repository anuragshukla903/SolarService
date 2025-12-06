using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolarService.Interface;
using SolarService.Models;

namespace SolarService.Infrastructure
{
    public class TenantProvider : ITenantProvider
    {
        private Tenant? _tenant;
        public int TenantId => _tenant?.Id ?? 0;
        public Tenant? Tenant => _tenant;
        public void SetTenant(Tenant tenant)
        {
            _tenant = tenant;
        }
    }

}