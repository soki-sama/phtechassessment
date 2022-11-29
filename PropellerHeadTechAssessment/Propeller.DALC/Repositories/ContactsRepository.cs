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
    public class ContactsRepository : IContactsRepository
    {
        private CustomerDbContext _customerDbContext;

        public ContactsRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        public async Task<bool> InsertCustomerAsync(Contact newContact)
        {
            await _customerDbContext.Contacts.AddAsync(newContact);
            var result = await _customerDbContext.SaveChangesAsync();
            return (result != 0);
        }

    }
}
