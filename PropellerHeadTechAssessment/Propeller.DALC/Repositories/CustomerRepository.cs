﻿using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Sqlite;
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
        private CustomerDbContext _customerDbContext;

        public CustomerRepository(CustomerDbContext customerDbContext)
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

        // TODO: Implement proper error handling routine
        public async Task<bool> InsertCustomer(Customer newCustomer)
        {
            // TODO: Check about trigger usage on sqlite to handle this
            newCustomer.CreatedOn = DateTime.UtcNow;
            newCustomer.LastModified = DateTime.UtcNow;

            var response = _customerDbContext.Customers.Add(newCustomer);
            var result = await _customerDbContext.SaveChangesAsync();
            return ( result != 0);
        }

    }
}
