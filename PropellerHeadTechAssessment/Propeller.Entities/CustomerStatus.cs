using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Entities
{
    public class CustomerStatus
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        public ICollection<Customer> Customers { get; set; }
    }
}
