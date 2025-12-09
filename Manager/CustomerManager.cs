using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarService.Data;
using SolarService.DataContract.Customer;
using SolarService.Models;

namespace SolarService.Manager;

public class CustomerManager
{
    private readonly AppDbContext _context;
    
    public CustomerManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> CreateCustomerAsync(CustomerCreateDto dto, int tenantId)
    {
        var customer = new Customer
        {
            FullName = dto.FullName,
            Mobile = dto.Mobile,
            Email = dto.Email,
            AlternateMobile = dto.AlternateMobile,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int id, int tenantId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);
        
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<List<CustomerResponseDto>> GetCustomersAsync(int tenantId, int skip = 0, int take = 20)
    {
        return await _context.Customers
            .Where(c => c.TenantId == tenantId)
            .OrderBy(c => c.FullName)
            .Skip(skip)
            .Take(take)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<Customer?> UpdateCustomerAsync(int id, CustomerUpdateDto dto, int tenantId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

        if (customer == null)
            return null;

        // Only update properties that are not null in the DTO
        if (dto.FullName != null) customer.FullName = dto.FullName;
        if (dto.Mobile != null) customer.Mobile = dto.Mobile;
        if (dto.Email != null) customer.Email = dto.Email;
        if (dto.AlternateMobile != null) customer.AlternateMobile = dto.AlternateMobile;
        if (dto.Address != null) customer.Address = dto.Address;
        if (dto.City != null) customer.City = dto.City;
        if (dto.State != null) customer.State = dto.State;
        if (dto.Pincode != null) customer.Pincode = dto.Pincode;
        
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteCustomerAsync(int id, int tenantId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CustomerResponseDto MapToDto(Customer customer) => new()
    {
        Id = customer.Id,
        FullName = customer.FullName,
        Mobile = customer.Mobile,
        Email = customer.Email,
        AlternateMobile = customer.AlternateMobile,
        Address = customer.Address,
        City = customer.City,
        State = customer.State,
        Pincode = customer.Pincode,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt,
        TenantId = customer.TenantId
    };
}