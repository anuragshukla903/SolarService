using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolarService.Data;
using SolarService.DataContract.Lead;
using SolarService.Manager;

namespace SolarService.Controllers.Lead
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly LeadManager _leadManager;
        public LeadController(LeadManager leadManager)
        {
            _leadManager = leadManager;
        }
        [HttpGet("getleads")]
        public async Task<IActionResult> GetLeads(string leadId, int skip, int take)
        {
            var leads = await _leadManager.GetLeadsByLeadIdAsync(leadId, skip, take);
            return Ok(leads);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] LeadCreateDto dto)
        {
            int tenantId = 1; // 🔥 Tenant system lagane baad yaha se milega
            var lead = await _leadManager.CreateLeadAsync(dto, tenantId);
            return Ok(lead);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(int id, LeadUpdateDto dto)
        {
            var updated = await _leadManager.UpdateLeadAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // ➤ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var deleted = await _leadManager.DeleteLeadAsync(id);
            if (!deleted) return NotFound();

            return Ok(new { message = "Lead deleted successfully" });
        }
    }
}