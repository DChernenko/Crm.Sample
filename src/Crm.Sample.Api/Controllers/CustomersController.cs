using Crm.Sample.Application.Dtos.Customers;
using Crm.Sample.Application.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Sample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllAsync(cancellationToken);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetByIdAsync(id, cancellationToken);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CreateCustomerDto createDto, CancellationToken cancellationToken)
        {
            var userId = 1;
            var customer = await _customerService.CreateAsync(createDto, userId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Put(int id, UpdateCustomerDto updateDto, CancellationToken cancellationToken)
        {
            var userId = 1;
            var customer = await _customerService.UpdateAsync(id, updateDto, userId, cancellationToken);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        //[HttpGet("check-email")]
        //public async Task<ActionResult<bool>> CheckEmail(string email, string excludeId = null)
        //{
        //var exists = await _customerService.EmailExistsAsync(email, excludeId);
        //return Ok(exists);
        //}
    }
}
