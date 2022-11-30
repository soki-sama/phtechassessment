using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Sqlite;
using Propeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private CustomerDbContext _customerDbContext;


        public NotesRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        /// <summary>
        /// Deletes a Note from a given customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="noteID"></param>
        /// <returns>True when the record was found and deleted, False otherwise</returns>
        public async Task<bool> DeleteNoteAsync(int customerID, int noteID)
        {
            // TODO: Add proper err handling
            var note = _customerDbContext.Notes.Where(x => x.CustomerID == customerID &&
                x.ID == noteID).FirstOrDefault();

            if (note != null)
            {
                _customerDbContext.Notes.Remove(note);
                var result = await _customerDbContext.SaveChangesAsync();

                return (result != 0);
            }

            return false;
        }

        // TODO: Add proper error handling
        public async Task<bool> SaveChangesAsync()
        {
            var res = await _customerDbContext.SaveChangesAsync();
            return (true);

        }

        public async Task<Note> InsertNoteAsync(Note newNote)
        {
            // TODO: Maybe I should return null if something went wrong or a tuple, think about it
            // TODO: Should I return the newly created object?
            _customerDbContext.Notes.Add(newNote);
            var result = await _customerDbContext.SaveChangesAsync();
            return newNote;
        }

        public async Task<Note?> RetrieveNoteAsync(int customerID, int noteID)
        {
            return await _customerDbContext.Notes
                .Where(c => c.CustomerID.Equals(customerID) && c.ID == noteID).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Note>> RetrieveNotesAsync(int customerID)
        {
            return await _customerDbContext.Notes
                .Where(c => c.CustomerID.Equals(customerID)).ToListAsync();
        }

        public async Task<bool> DeleteNotesAsync(int customerID)
        {
            var notes = _customerDbContext.Notes.Where(x => x.CustomerID == customerID).ToList();

            _customerDbContext.Notes.RemoveRange(notes);
            await _customerDbContext.SaveChangesAsync();

            return true;
        }
    }
}
