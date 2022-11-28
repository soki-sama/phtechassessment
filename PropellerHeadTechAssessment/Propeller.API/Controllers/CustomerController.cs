using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;

namespace Propeller.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;

        public CustomerController(
            ICustomerRepository customerRepository,
            IMapper mapper,
            ILogger<CustomerController> logger
        )
        {
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> RetrieveCustomers()
        {
            var customers = await _customerRepo.RetrieveCustomersAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        /// <summary>
        /// Creates a new Customer 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request)
        {
            var newCustomer = _mapper.Map<Customer>(request);
            var result = await _customerRepo.InsertCustomer(newCustomer);
            return new OkResult();
        }

    }
}
