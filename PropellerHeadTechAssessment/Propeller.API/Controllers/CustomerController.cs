using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.Models;

namespace Propeller.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepo;

        public CustomerController(
            ICustomerRepository customerRepository,
            ILogger<CustomerController> logger
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        /// <summary>
        /// Retrieves all Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> RetrieveCustomers()
        {
            var customers = await _customerRepo.RetrieveCustomersAsync();
            return Ok(customers);
        }


    }
}
