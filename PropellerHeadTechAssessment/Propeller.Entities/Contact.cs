using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Entities
{
    public class Contact
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]  
        public string LastName { get; set; }


        public string EMail { get; set; }

        public string PhoneNumber { get; set; }


        public ICollection<Customer> Customers { get; set; }
    }
}
