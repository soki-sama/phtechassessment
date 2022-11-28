using Microsoft.EntityFrameworkCore;
using Propeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Sqlite
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
          : base(options) { }

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Contact> Contacts { get; set; } = null!;

        public DbSet<Note> Notes { get; set; } = null!;

    }
}
