using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolarService.DataContract;
using SolarService.DataContract.Customer;
using SolarService.Manager;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SolarService.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

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
    public async Task<ActionResult<Response>> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var tenantId = GetTenantId();
            var skip = (page - 1) * pageSize;
            var customers = await _customerManager.GetCustomersAsync(tenantId, skip, pageSize);
            return Ok(new Response
            {
                Status = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Customers retrieved successfully",
                Data = customers
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = false,
                Code = StatusCodes.Status500InternalServerError,
                Message = $"Error retrieving customers: {ex.Message}",
                Data = null
            });
        }
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetCustomer(int id)
    {
        try
        {
            var tenantId = GetTenantId();
            var customer = await _customerManager.GetCustomerByIdAsync(id, tenantId);
            
            if (customer == null)
            {
                return NotFound(new Response
                {
                    Status = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Customer not found",
                    Data = null
                });
            }

            return Ok(new Response
            {
                Status = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Customer retrieved successfully",
                Data = customer
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = false,
                Code = StatusCodes.Status500InternalServerError,
                Message = $"Error retrieving customer: {ex.Message}",
                Data = null
            });
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Response>> CreateCustomer(CustomerCreateDto createDto)
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
            var tenantId = GetTenantId();
            var customer = await _customerManager.CreateCustomerAsync(createDto, tenantId);
            var createdCustomer = await _customerManager.GetCustomerByIdAsync(customer.Id, tenantId);
            
            return CreatedAtAction(
                nameof(GetCustomer), 
                new { id = customer.Id },
                new Response
                {
                    Status = true,
                    Code = (int)HttpStatusCode.Created,
                    Message = "Customer created successfully",
                    Data = createdCustomer
                });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = false,
                Code = StatusCodes.Status500InternalServerError,
                Message = $"Error creating customer: {ex.Message}",
                Data = null
            });
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Response>> UpdateCustomer(int id, CustomerUpdateDto updateDto)
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
            var tenantId = GetTenantId();
            var customer = await _customerManager.UpdateCustomerAsync(id, updateDto, tenantId);
            
            if (customer == null)
            {
                return NotFound(new Response
                {
                    Status = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Customer not found",
                    Data = null
                });
            }

            return Ok(new Response
            {
                Status = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Customer updated successfully",
                Data = customer
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = false,
                Code = StatusCodes.Status500InternalServerError,
                Message = $"Error updating customer: {ex.Message}",
                Data = null
            });
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeleteCustomer(int id)
    {
        try
        {
            var tenantId = GetTenantId();
            var result = await _customerManager.DeleteCustomerAsync(id, tenantId);
            
            if (!result)
            {
                return NotFound(new Response
                {
                    Status = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Customer not found",
                    Data = null
                });
            }

            return Ok(new Response
            {
                Status = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Customer deleted successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = false,
                Code = StatusCodes.Status500InternalServerError,
                Message = $"Error deleting customer: {ex.Message}",
                Data = null
            });
        }
    }
}