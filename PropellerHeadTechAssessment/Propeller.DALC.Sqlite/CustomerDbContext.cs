using Microsoft.EntityFrameworkCore;
using Propeller.Entities;
using Propeller.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Sqlite
{
    public class CustomerDbContext : DbContext, ICustomerDbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
          : base(options) { }

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Contact> Contacts { get; set; } = null!;

        public DbSet<Note> Notes { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Status catalog
            modelBuilder.Entity<CustomerStatus>().HasData(
                new CustomerStatus { ID = 1, State = "prospective" }, // default
                new CustomerStatus { ID = 2, State = "current" },
                new CustomerStatus { ID = 3, State = "non-active" }
                );

            // Seed customers for testing filters and pagination
            modelBuilder.Entity<Customer>().HasData(
                new Customer { ID = 1, Name = "Customer One", CustomerStatusID = 1 },
                new Customer { ID = 2, Name = "Customer Two", CustomerStatusID = 1 },
                new Customer { ID = 3, Name = "Customer Three", CustomerStatusID = 1 },
                new Customer { ID = 4, Name = "Customer Four", CustomerStatusID = 1 },
                new Customer { ID = 5, Name = "Customer Five", CustomerStatusID = 1 },
                new Customer { ID = 6, Name = "Customer Six", CustomerStatusID = 1 },
                new Customer { ID = 7, Name = "Customer Seven", CustomerStatusID = 1 },
                new Customer { ID = 8, Name = "Customer Nine", CustomerStatusID = 1 },
                new Customer { ID = 9, Name = "Customer Ten", CustomerStatusID = 1 },
                new Customer { ID = 10, Name = "Customer Eleven", CustomerStatusID = 1 },
                new Customer { ID = 11, Name = "Customer Twelve", CustomerStatusID = 1 },
                new Customer { ID = 12, Name = "Customer Thirteen", CustomerStatusID = 1 },
                new Customer { ID = 13, Name = "Customer Fourteen", CustomerStatusID = 1 }
                );

            modelBuilder.Entity<Note>().HasData(
                    new Note { CustomerID = 1, ID = 1, Text = "Note 1", TimeStamp = DateTime.UtcNow },
                    new Note { CustomerID = 1, ID = 2, Text = "Note 2", TimeStamp = DateTime.UtcNow }
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
