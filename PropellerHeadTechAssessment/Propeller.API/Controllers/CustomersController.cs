using AutoMapper;
using CSharpVitamins;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
using Propeller.DALC.Interfaces;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;
using Propeller.Shared;
using System.Text.Json;

namespace Propeller.API.Controllers
{
    // TODO: Add model validation
    // TODO: Add proper return type for requests
    // TODO: Cleanup

    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly INotesRepository _notesRepository;
        private readonly IMapper _mapper;

        private int maxPageSize = 50;
        private int minPageSize = 5;

        public CustomersController(
            ICustomerRepository customerRepository,
            INotesRepository notesRepository,
            IMapper mapper,
            ILogger<CustomersController> logger
        )
        {
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>>
            RetrieveCustomers(
                [FromQuery(Name = "q")] string? criteria,
                [FromQuery(Name = "pn")] int pageNumber = 1,
                [FromQuery(Name = "ps")] int pageSize = 25
            )
        {
            if (pageSize < minPageSize)
            {
                pageSize = minPageSize;
            }
            else if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            // IEnumerable<Customer> customers = new List<Customer>();

            var (customers, paginationMeta) = await _customerRepo.RetrieveCustomersAsync(criteria, pageNumber, pageSize);


            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMeta));

            //if (string.IsNullOrEmpty(criteria))
            //{
            //    customers = await _customerRepo.RetrieveCustomersAsync();
            //}
            //else
            //{

            //}
            List<CustomerDto> customersDto = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                customersDto.Add(
                    new CustomerDto
                    {
                        ID = customer.ID.Obfuscate(),
                        Name = customer.Name
                    });
            }

            return Ok(customersDto);

            // TODO Configure the mapper to use the obfuscator
            // return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));

            // return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> RetrieveCustomer(string id)
        {
            var customer = await _customerRepo.RetrieveCustomerAsync(id.Deobfuscate());

            if (customer == null)
            {
                return NotFound();
            }

            // Attach notes
            var notes = await _notesRepository.RetrieveNotesAsync(id.Deobfuscate());

            var customerDto = _mapper.Map<CustomerDto>(customer);
            var notesDto = _mapper.Map<IEnumerable<NoteDto>>(notes);

            customerDto.Notes = notesDto;

            return Ok(customerDto);
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
            return Ok(_mapper.Map<CustomerDto>(result));
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id,
            [FromQuery(Name = "fd")] string forceDelete = "n")
        {
            // Retrieve Notes
            var existingNotes = await _notesRepository.RetrieveNotesAsync(id);

            if (existingNotes.Any() && forceDelete == "n")
            {
                return Forbid(); // TODO: Decide on proper response code for this
            }


            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerRepo.DeleteCustomerAsync(existingCustomer);

            return Ok();
        }


    }
}
