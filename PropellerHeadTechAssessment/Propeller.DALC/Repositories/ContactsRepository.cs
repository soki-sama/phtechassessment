using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Sqlite;
using Propeller.Entities;
using Propeller.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private CustomerDbContext _customerDbContext;

        public ContactsRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newContact"></param>
        /// <returns></returns>
        public async Task<Contact> InsertContactAsync(Contact newContact)
        {
            await _customerDbContext.Contacts.AddAsync(newContact);
            var result = await _customerDbContext.SaveChangesAsync();
            return newContact;
        }

        public async Task<Contact?> RetrieveContact(int contactId)
        {
            return await _customerDbContext.Contacts.Include(x => x.Customers)
                .Where(x => x.ID == contactId).FirstOrDefaultAsync();

            // return await _customerDbContext.Contacts.Where(x => x.ID == contactId).FirstOrDefaultAsync();
        }

        public async Task<List<Contact>> RetrieveContactsByCustomerId(int customerId)
        {
            // TODO: Optimize this query?
            return await _customerDbContext.Contacts.Where(x => x.Customers.Where(x => x.ID == customerId).Count() > 0).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Contact> contacts, PaginationMeta pagination)> 
            RetrieveContactsAsync(string? query, int pageNumber, int pageSize)
        {
            var tempColl = _customerDbContext.Contacts as IQueryable<Contact>;

            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filter = filter.Trim();
            //    tempColl = tempColl.Where(x => x.Name.Equals(filter));
            //}

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                // TODO: Add filtering for Last name too
                tempColl = tempColl.Where(x => x.FirstName.ToUpper().Contains(query.ToUpper()));
            }

            int totalRecords = await tempColl.CountAsync();

            var paginationMeta = new PaginationMeta(totalRecords, pageSize, pageNumber);

            // TODO: Add sorting
            var records = await tempColl
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (records, paginationMeta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteContactAsync(Contact contact)
        {
            _customerDbContext.Contacts.Remove(contact);
            var result = await _customerDbContext.SaveChangesAsync();
            return (result != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Contact?> RetrieveContact(Contact contact)
        {
            return await _customerDbContext.Contacts.Include(x => x.Customers)
                .Where(x => 
                    x.FirstName == contact.FirstName &&
                    x.LastName == contact.LastName && 
                    x.EMail== contact.EMail && 
                    x.PhoneNumber == contact.PhoneNumber
                ).FirstOrDefaultAsync();
        }

    }
}
