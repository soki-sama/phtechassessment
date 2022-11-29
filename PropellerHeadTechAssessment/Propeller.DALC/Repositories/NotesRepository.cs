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

        public async Task<bool> DeleteNoteAsync(int customerID, int noteID)
        {
            // TODO: Add proper err handling
            var note = _customerDbContext.Notes.Where(x => x.CustomerID == customerID &&
                x.ID == noteID).FirstOrDefault();

            if (note != null)
            {
                _customerDbContext.Notes.Remove(note);
                await _customerDbContext.SaveChangesAsync();
            }

            return true;
        }

        // TODO: Add proper error handling
        public async Task<bool> SaveChangesAsync()
        {
            var res = await _customerDbContext.SaveChangesAsync();
            return (true);

        }

        public async Task<bool> InsertNoteAsync(Note newNote)
        {
            // TODO: Should I return the newly created object?
            _customerDbContext.Notes.Add(newNote);
            var result = await _customerDbContext.SaveChangesAsync();
            return (result != 0);
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

    }
}
