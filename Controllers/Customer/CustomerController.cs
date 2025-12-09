using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolarService.DataContract.Customer;
using SolarService.Manager;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SolarService.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace SolarService.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class CustomerController : ControllerBase
{
    private readonly CustomerManager _customerManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerController(CustomerManager customerManager, IHttpContextAccessor httpContextAccessor)
    {
        _customerManager = customerManager;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetTenantId()
    {
        var tenantProvider = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ITenantProvider>();
        if (tenantProvider.TenantId > 0)
        {
            return tenantProvider.TenantId;
        }
        throw new UnauthorizedAccessException("Invalid tenant information. Please ensure the 'x-tenant-id' header is set with a valid tenant ID.");
    }

    /// <summary>
    /// Get paginated list of customers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var tenantId = GetTenantId();
        var skip = (page - 1) * pageSize;
        var customers = await _customerManager.GetCustomersAsync(tenantId, skip, pageSize);
        return Ok(customers);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id)
    {
        var tenantId = GetTenantId();
        var customer = await _customerManager.GetCustomerByIdAsync(id, tenantId);
        
        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        return Ok(customer);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer(CustomerCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var tenantId = GetTenantId();
            var customer = await _customerManager.CreateCustomerAsync(createDto, tenantId);
            
            return CreatedAtAction(
                nameof(GetCustomer), 
                new { id = customer.Id }, 
                await _customerManager.GetCustomerByIdAsync(customer.Id, tenantId));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tenantId = GetTenantId();
        var customer = await _customerManager.UpdateCustomerAsync(id, updateDto, tenantId);
        
        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var tenantId = GetTenantId();
        var result = await _customerManager.DeleteCustomerAsync(id, tenantId);
        
        if (!result)
        {
            return NotFound(new { message = "Customer not found" });
        }

        return NoContent();
    }
}