using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.Entities;
using Propeller.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private ICustomerDbContext _customerDbContext;

        public CustomerRepository(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        public async Task<Customer?> RetrieveCustomerAsync(int customerId)
        {
            return await _customerDbContext.Customers.FirstOrDefaultAsync(c => c.ID.Equals(customerId));

        }

        public async Task<IEnumerable<Customer>> RetrieveCustomersAsync()
        {
            return await _customerDbContext.Customers.ToListAsync();
        }
    }
}
