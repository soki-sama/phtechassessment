﻿using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Repositories;
using Propeller.Entities;
using Propeller.Mappers;
using Propeller.Models;
using Propeller.Shared;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Propeller.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/notes")]
    public class NotesController : PropellerControllerBase
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet("{cid}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> RetrieveCustomerNotes(string cid)
        {
            int customerId = -1;

            try
            {
                customerId = Obfuscator.DeobfuscateId(cid);

                if (customerId == -1)
                {
                    return BadRequest();
                }

                var notes = await _notesRepository.RetrieveNotesAsync(customerId);
                return Ok(_mapper.Map<IEnumerable<NoteDto>>(notes));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Retrieving Customer Notes. CID:{customerId}");
                return StatusCode(500, "Unable to Retrieve Notes");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet("{cid}/{noteId}", Name = "GetNote")]
        public async Task<ActionResult<NoteDto>> RetrieveCustomerNote(string cid, int noteId)
        {
            int customerId = -1;

            try
            {

                customerId = Obfuscator.DeobfuscateId(cid);

                if (customerId == -1)
                {
                    return BadRequest();
                }

                // TODO: Add validation for zero

                var note = await _notesRepository.RetrieveNoteAsync(customerId, noteId);

                if (note == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<NoteDto>(note));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Retrieving Customer Note. NID:{noteId}");
                return StatusCode(500, "Unable to Retrieve Note");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        [HttpPost("{cid}")]
        public async Task<ActionResult<NoteDto>> AddCustomerNote(string cid,
            [FromForm] string? noteText)
        {
            int customerId = -1;

            try
            {

                if (!ValidateAdmin())
                {
                    return Forbid();
                }

                if (string.IsNullOrEmpty(noteText))
                {
                    return UnprocessableEntity("Note Text Required"); // TODO: Is this the best error code to return?
                }

                if (noteText.Length > 500)
                {
                    return UnprocessableEntity("Length Exceeded");
                }

                customerId = Obfuscator.DeobfuscateId(cid);

                if (customerId == -1)
                {
                    return BadRequest();
                }

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

                var createdNote = await _notesRepository.InsertNoteAsync(note);
                return CreatedAtRoute("GetNote",
                       new { cid = cid, noteId = note.ID.ToString() },
                      _mapper.Map<NoteDto>(createdNote));

                // return Ok(_mapper.Map<NoteDto>(createdNote));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Updating Customer Note for Customer: {customerId}");
                return StatusCode(500, "Unable to Update Note");
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="noteId"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        [HttpPut("{cid}/{noteId}")]
        public async Task<ActionResult> UpdateCustomerNote(string cid,
            int noteId, 
            [FromForm] string noteText)
        {
            int customerId = -1;

            try
            {
                if (!ValidateAdmin())
                {
                    return Forbid();
                }

                customerId = cid.Deobfuscate();

                if (customerId == -1)
                {
                    return BadRequest();
                }

                // Validate Note exists
                var existingNote = await _notesRepository.RetrieveNoteAsync(customerId, noteId);

                if (existingNote == null)
                {
                    return NotFound();
                }

                existingNote.Text = noteText;

                await _notesRepository.SaveChangesAsync();
                return NoContent(); // TODO: Should I return Ok iinstead?

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Updating Customer Note for Customer: {customerId}");
                return StatusCode(500, "Unable to Update Note");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpDelete("{cid}/{noteId}")]
        public async Task<ActionResult> DeleteCustomerNote(string cid, int noteId)
        {
            int customerId = -1;

            try
            {

                if (!ValidateAdmin())
                {
                    return Forbid();
                }

                customerId = Obfuscator.DeobfuscateId(cid);

                if (customerId == -1)
                {
                    return BadRequest();
                }

                bool result = await _notesRepository.DeleteNoteAsync(customerId, noteId);

                if (result)
                {
                    return Ok(); // All good
                }
                else
                {
                    return NoContent(); // Dismissable 
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Deleting Customer Note for Customer: {customerId}");
                return StatusCode(500, "Unable to Delete Note");
            }

        }

    }

}
