using Propeller.Entities;
using Propeller.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Interfaces
{
    public interface ICustomerRepository
    {

        // Task<IEnumerable<Customer>> RetrieveCustomersAsync();

        Task<(IEnumerable<Customer> customers, PaginationMeta pagination)> RetrieveCustomersAsync(string? query, int pageNumber, int pageSize);

        // Marked Customer as nullable to allow the returned object to be
        // evaluated at the upper most level so theAPI can report back
        // This is to prevent exception bubbling or validation implementation at the lower levels
        // addding complexity and diminishing maintainability
        Task<Customer?> RetrieveCustomerAsync(int customerId);

        Task<Customer> InsertCustomerAsync(Customer newCustomer);

        Task<Customer?> RetrieveCustomerByNameAsync(string customerName);

        Task<bool> SaveChangesAsync();

        Task<bool> DeleteCustomerAsync(Customer customer);

        Task<IEnumerable<Customer>> RetrieveCustomersByContact(int contaciId);

    }
}
