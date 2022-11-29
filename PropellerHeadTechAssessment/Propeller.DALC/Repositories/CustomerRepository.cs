using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Sqlite;
using Propeller.Entities;
using Propeller.Entities.Interface;
using Propeller.Models.Metadata;
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

        //public async Task<IEnumerable<Customer>> RetrieveCustomersAsync()
        //{
        //    return await _customerDbContext.Customers.ToListAsync();
        //}

        // TODO: Implement proper error handling routine
        public async Task<bool> InsertCustomerAsync(Customer newCustomer)
        {
            // TODO: Should I return the newly created object?
            // TODO: Check about trigger usage on sqlite to handle this
            newCustomer.CreatedOn = DateTime.UtcNow;
            newCustomer.LastModified = DateTime.UtcNow;

            var response = _customerDbContext.Customers.Add(newCustomer);
            var result = await _customerDbContext.SaveChangesAsync();
            return (result != 0);
        }

        public async Task<bool> SaveChangesAsync()
        {
            var res = await _customerDbContext.SaveChangesAsync();
            return (true);

        }

        public async Task<bool> DeleteCustomerAsync(Customer customer)
        {
            _customerDbContext.Customers.Remove(customer);
            var result = await _customerDbContext.SaveChangesAsync();
            return (result != 0);
        }

        public async Task<(IEnumerable<Customer> customers, PaginationMeta pagination)>
            RetrieveCustomersAsync(string? query, int pageNumber, int pageSize)
        {

            //if (string.IsNullOrEmpty(filter) &&
            //    string.IsNullOrEmpty(query))
            //{
            //    return await RetrieveCustomersAsync();
            //}

            var tempColl = _customerDbContext.Customers as IQueryable<Customer>;

            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filter = filter.Trim();
            //    tempColl = tempColl.Where(x => x.Name.Equals(filter));
            //}

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                tempColl = tempColl.Where(x => x.Name.ToUpper().Contains(query.ToUpper()));
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
    }
}
