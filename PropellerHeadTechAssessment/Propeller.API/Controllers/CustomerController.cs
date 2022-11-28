using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;

namespace Propeller.API.Controllers
{
    // TODO: Add model validation
    // TODO: Add proper return type for requests
    // TODO: Cleanup

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


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> RetrieveCustomer(int id)
        {
            var customer = await _customerRepo.RetrieveCustomerAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, UpdateCustomerRequest request)
        {
            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(id);
            
            if (existingCustomer == null)
            {
                return NotFound();
            }

            _mapper.Map(request, existingCustomer);

            await _customerRepo.SaveChangesAsync();

            return Ok();

        }

        /// <summary>
        /// Creates a new Customer 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request)
        {
            // TODO: Add proper validation
            // TODO: Check why the required validation on the request is not working properly

            var newCustomer = _mapper.Map<Customer>(request);
            var result = await _customerRepo.InsertCustomerAsync(newCustomer);
            return new OkResult();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateCustomer(int id, 
            JsonPatchDocument<UpdateCustomerRequest> requestPatch)
        {

            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            UpdateCustomerRequest customerPatch = _mapper.Map<UpdateCustomerRequest>(existingCustomer);

            requestPatch.ApplyTo(customerPatch, ModelState);

            _mapper.Map(customerPatch, existingCustomer);

            var result = await _customerRepo.SaveChangesAsync();
            return Ok();

        }


    }
}
