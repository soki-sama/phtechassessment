using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Repositories;
using Propeller.Entities;
using Propeller.Models;
using System.Linq;
using System.Xml.Linq;

namespace Propeller.API.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/status")]
    public class CustomerStatusController : ControllerBase
    {
        private ICustomerStatusRepository _customerStatusRepository;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;

        public CustomerStatusController(IContactsRepository contactsRepo,
                ICustomerStatusRepository customerStatusRepository,
                IMapper mapper,
                ILogger<CustomersController> logger
        )
        {
            _customerStatusRepository = customerStatusRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerStatus>>> RetrieveCustomerStatuses()
        {
            try
            {
                var result = await _customerStatusRepository.RetrieveStatusesAsync(); 
                return Ok(_mapper.Map<IEnumerable<CustomerStatusDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred when Retrieving Customer Statuses");
                return StatusCode(500, "Unable to retrieve Statuses");
            }
        }

    }
}
