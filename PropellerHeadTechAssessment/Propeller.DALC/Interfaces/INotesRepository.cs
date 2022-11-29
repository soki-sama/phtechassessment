using Propeller.Entities;
using Propeller.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Interfaces
{
    public interface INotesRepository
    {

        Task<IEnumerable<Note>> RetrieveNotesAsync(int customerID);

        Task<Note?> RetrieveNoteAsync(int customerID, int noteID);

        Task<bool> DeleteNoteAsync(int customerID, int noteID);

        Task<bool> DeleteNotesAsync(int customerID);

        Task<Note> InsertNoteAsync(Note newNote);

        Task<bool> SaveChangesAsync();
    }
}
