using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolarService.Models;

namespace SolarService.DataContract.Lead
{

    public class LeadCreateDto
    {
        public required string CustomerName { get; set; }
        public required string Mobile { get; set; }
        public string? Email { get; set; }
        public required string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? LeadSource { get; set; }
        public double RequiredKW { get; set; }
        public int? AssignedToId { get; set; }
        public string? CustomerRemark { get; set; }
    }
public class LeadUpdateDto
{
    public required string CustomerName { get; set; }
    public required string Mobile { get; set; }
    public string? Email { get; set; }
    public required string Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? LeadSource { get; set; }
    public double RequiredKW { get; set; }
    public int? AssignedToId { get; set; }
    public LeadStatus Status { get; set; }
    public string? CustomerRemark { get; set; }
}


}