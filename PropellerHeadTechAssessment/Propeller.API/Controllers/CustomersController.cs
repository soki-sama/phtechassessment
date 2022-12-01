﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Repositories;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;
using Propeller.Shared;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Propeller.API.Controllers
{
    // TODO: Add model validation
    // TODO: Add proper return type for requests
    // TODO: Cleanup

    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly INotesRepository _notesRepository;
        private readonly IContactsRepository _contactsRepository;
        private readonly ICustomerStatusRepository _customerStatusRepository;
        private readonly IMapper _mapper;

        private int maxPageSize = 50;
        private int minPageSize = 5;

        public CustomersController(
            ICustomerRepository customerRepository,
            INotesRepository notesRepository,
            IContactsRepository contactsRepository,
            ICustomerStatusRepository customerStatusRepository,
            IMapper mapper,
            ILogger<CustomersController> logger
        )
        {
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
            _contactsRepository = contactsRepository ?? throw new ArgumentNullException(nameof(contactsRepository));
            _customerStatusRepository = customerStatusRepository ?? throw new ArgumentNullException(nameof(customerStatusRepository));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> RetrieveCustomer(string id)
        {
            int customerId = -1;

            try
            {
                customerId = id.Deobfuscate();

                if (customerId == -1)
                {
                    return BadRequest();
                }

                var customer = await _customerRepo.RetrieveCustomerAsync(id.Deobfuscate());

                if (customer == null)
                {
                    return NotFound();
                }

                var customerDto = _mapper.Map<CustomerDto>(customer);

                // TODO make this next step optional
                // Attach notes
                var notes = await _notesRepository.RetrieveNotesAsync(customerId);
                var notesDto = _mapper.Map<IEnumerable<NoteDto>>(notes);

                customerDto.Notes = notesDto;

                // Retrieve Contacts
                var contacts = await _contactsRepository.RetrieveContactsByCustomerId(customerId);
                var contactsDto = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                customerDto.Contacts = contactsDto;

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Retrieving Customer. CID:{customerId}");
                return StatusCode(500, "Unable to retrieve Customer");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(string id, UpdateCustomerRequest request)
        {
            int customerId = -1;

            try
            {
                customerId = id.Deobfuscate();

                if (customerId == -1)
                {
                    return BadRequest();
                }

                var existingCustomer = await _customerRepo.RetrieveCustomerAsync(customerId);

                if (existingCustomer == null)
                {
                    return NotFound();
                }

                _mapper.Map(request, existingCustomer);

                await _customerRepo.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Retrieving Customer. CID:{customerId}");
                return StatusCode(500, "Unable to update Customer");
            }

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

            try
            {
                // Validate the customer doesn't exist
                var preExisting = await _customerRepo.RetrieveCustomerByNameAsync(request.Name);

                if (preExisting != null)
                {
                    return Conflict();
                }

                var newCustomer = _mapper.Map<Customer>(request);
                var result = await _customerRepo.InsertCustomerAsync(newCustomer);

                return Ok(_mapper.Map<CustomerDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Creating Customer");
                return StatusCode(500, "Unable to create Customer");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestPatch"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        [HttpPut("{cid}/status/{sid}")]
        public async Task<ActionResult> ChangeCustomerStatus(string cid, string sid)
        {
            int customerId = -1;
            int statusId = -1;

            customerId = cid.Deobfuscate();
            statusId = sid.Deobfuscate();

            if (customerId == -1 || statusId == -1)
            {
                return BadRequest();
            }

            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(customerId);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Verify valid Status Id
            if (!await _customerStatusRepository.ValidateStatusExists(statusId))
            {
                return NotFound(); // TODO: Pick a proper error code for this
            }

            existingCustomer.CustomerStatusID = statusId;
            var result = await _customerRepo.SaveChangesAsync();

            if (!result)
            {
                return NoContent(); // This would happen if the record was not saved TODO: Check best strategy to address this
            }

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forceDelete"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(string id,
            [FromQuery(Name = "fd")] string? forceDelete = "n")
        {

            int customerId = Obfuscator.DeobfuscateId(id);

            if (customerId == -1)
            {
                return BadRequest();
            }

            // Retrieve Notes
            var existingNotes = await _notesRepository.RetrieveNotesAsync(customerId);

            // TODO: Add retrieval of contacts

            if (existingNotes.Any() && forceDelete != "y")
            {
                return Forbid(); // TODO: Decide on proper response code for this
            }


            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(customerId);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerRepo.DeleteCustomerAsync(existingCustomer);

            return Ok();
        }


    }
}
