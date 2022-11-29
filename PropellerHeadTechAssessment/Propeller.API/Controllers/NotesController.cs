using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Repositories;
using Propeller.Entities;
using Propeller.Models;

namespace Propeller.API.Controllers
{

    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly INotesRepository _notesRepository;
        private readonly IMapper _mapper;

        public NotesController(
            ICustomerRepository customerRepository,
            INotesRepository notesRepository,
            IMapper mapper,
            ILogger<NotesController> logger
        )
        {
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
            _customerRepo = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerDto>> RetrieveCustomerNotes(int customerId)
        {
            var notes = await _notesRepository.RetrieveNotesAsync(customerId);
            return Ok(_mapper.Map<IEnumerable<NoteDto>>(notes));
        }

        [HttpPost("{customerId}")]
        public async Task<ActionResult> AddCustomerNote(int customerId,
            [FromForm] string noteText)
        {

            // Validate Customr exists
            var existingCustomer = await _customerRepo.RetrieveCustomerAsync(customerId);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            Note note = new Note
            {
                CustomerID = customerId,
                Text = noteText,
                TimeStamp = DateTime.UtcNow
            };

            await _notesRepository.InsertNoteAsync(note);
            return Ok();
        }

        [HttpPut("{customerId}/{noteId}")]
        public async Task<ActionResult> UpdateCustomerNote(int customerId,
            int noteId, [FromForm] string noteText)
        {

            // Make sure the note exists
            var existingNote = await _notesRepository.RetrieveNoteAsync(customerId, noteId);

            if (existingNote == null)
            {
                return NotFound();
            }

            existingNote.Text = noteText;

            await _notesRepository.SaveChangesAsync();
            return NoContent(); // TODO: Should I return Ok iinstead?
        }

        [HttpDelete("{customerId}/{noteId}")]
        public async Task<ActionResult> DeleteCustomerNote(int customerId, int noteId)
        {
            await _notesRepository.DeleteNoteAsync(customerId, noteId);
            return NoContent();
        }

    }

}
