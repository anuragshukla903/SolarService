using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarService.Data;
using SolarService.DataContract.Lead;
using SolarService.Models;

namespace SolarService.Manager
{
    public class LeadManager
    {

        private readonly AppDbContext _context;
        public LeadManager(AppDbContext context)
        {
            _context = context;
        }
        private async Task<string> GenerateLeadIdAsync()
        {
            int count = await _context.Leads.CountAsync() + 1;
            return $"LD{DateTime.UtcNow.Year}{count:0000}";
        }
        public async Task<Lead> CreateLeadAsync(LeadCreateDto dto, int tenantId)
        {
            var lead = new Lead
            {
                LeadId = await GenerateLeadIdAsync(),
                CustomerName = dto.CustomerName,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                LeadSource = dto.LeadSource,
                RequiredKW = dto.RequiredKW,
                AssignedToId = dto.AssignedToId,
                CustomerRemark = dto.CustomerRemark,
                TenantId = tenantId,
                Status = LeadStatus.New
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }
        public async Task<List<Lead>> GetLeadsByLeadIdAsync(string leadId, int skip, int take)
        {
            var query = _context.Leads.AsQueryable();

            if (leadId != "0")
            {
                query = query.Where(l => l.LeadId == leadId);
            }
            return await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        public async Task<Lead?> UpdateLeadAsync(int id, LeadUpdateDto dto)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return null;

            lead.CustomerName = dto.CustomerName;
            lead.Mobile = dto.Mobile;
            lead.Email = dto.Email;
            lead.Address = dto.Address;
            lead.Latitude = dto.Latitude;
            lead.Longitude = dto.Longitude;
            lead.LeadSource = dto.LeadSource;
            lead.RequiredKW = dto.RequiredKW;
            lead.AssignedToId = dto.AssignedToId;
            lead.Status = dto.Status;
            lead.CustomerRemark = dto.CustomerRemark;
            lead.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return lead;
        }

        // 🔹 Delete Lead
        public async Task<bool> DeleteLeadAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}