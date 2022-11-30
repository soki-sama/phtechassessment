using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.Entities;
using Propeller.Models.Requests;
using Propeller.Shared;

namespace Propeller.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;

        private readonly IContactsRepository _contactsRepo;
        private readonly ICustomerRepository _customerRepo;

        public ContactsController(IContactsRepository contactsRepo,
            ICustomerRepository customerRepository,
            ILogger<CustomersController> logger
        )
        {
            _contactsRepo = contactsRepo ?? throw new ArgumentNullException(nameof(contactsRepo));
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult> AddCustomerContacts(CreateContactRequest request)
        {
            // TODO: Add validation to either have email and or phone

            try
            {

                var customer = await _customerRepo.RetrieveCustomerAsync(request.CustomerID.Deobfuscate());

                if (customer == null)
                {
                    throw new Exception("TEMPORARY EX");
                }

                Contact contact = new Contact
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Customers = new List<Customer> { customer },
                    EMail = request.Email,
                    PhoneNumber = request.Phone
                };

                var r = await _contactsRepo.InsertCustomerAsync(contact);

                // Validate Customr exists
                //var existingCustomer = await _ _customerRepo.RetrieveCustomerAsync(customerId);

                //if (existingCustomer == null)
                //{
                //    return NotFound();
                //}

                //Note note = new Note
                //{
                //    CustomerID = customerId,
                //    Text = noteText,
                //    TimeStamp = DateTime.UtcNow
                //};

                //await _notesRepository.InsertNoteAsync(note);
                return Ok();

            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
