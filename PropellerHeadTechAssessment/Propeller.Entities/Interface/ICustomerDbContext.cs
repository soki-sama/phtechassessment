using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Entities.Interface
{
    public interface ICustomerDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Note> Notes { get; set; }
    }
}
