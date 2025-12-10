using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolarService.Data;
using SolarService.DataContract;
using SolarService.DataContract.Lead;
using SolarService.Manager;
using System.Net;

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
        public async Task<ActionResult<Response>> GetLeads(string leadId, int skip, int take)
        {
            try
            {
                var leads = await _leadManager.GetLeadsByLeadIdAsync(leadId, skip, take);
                return Ok(new Response
                {
                    Status = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Leads retrieved successfully",
                    Data = leads
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = false,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Error retrieving leads: {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpPost]
        public async Task<ActionResult<Response>> CreateLead([FromBody] LeadCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    Status = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid model state",
                    Data = ModelState
                });
            }

            try
            {
                int tenantId = 1; // 🔥 Tenant system lagane baad yaha se milega
                var lead = await _leadManager.CreateLeadAsync(dto, tenantId);
                
                return CreatedAtAction(
                    nameof(GetLeads), 
                    new { leadId = lead.LeadId, skip = 0, take = 10 },
                    new Response
                    {
                        Status = true,
                        Code = (int)HttpStatusCode.Created,
                        Message = "Lead created successfully",
                        Data = lead
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = false,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Error creating lead: {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdateLead(int id, [FromBody] LeadUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    Status = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid model state",
                    Data = ModelState
                });
            }

            try
            {
                var updated = await _leadManager.UpdateLeadAsync(id, dto);
                if (updated == null)
                {
                    return NotFound(new Response
                    {
                        Status = false,
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Lead not found",
                        Data = null
                    });
                }

                return Ok(new Response
                {
                    Status = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lead updated successfully",
                    Data = updated
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = false,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Error updating lead: {ex.Message}",
                    Data = null
                });
            }
        }

        // ➤ DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteLead(int id)
        {
            try
            {
                var deleted = await _leadManager.DeleteLeadAsync(id);
                if (!deleted)
                {
                    return NotFound(new Response
                    {
                        Status = false,
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Lead not found",
                        Data = null
                    });
                }

                return Ok(new Response
                {
                    Status = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lead deleted successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = false,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Error deleting lead: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}