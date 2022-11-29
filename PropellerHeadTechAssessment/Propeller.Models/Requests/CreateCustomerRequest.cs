using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Models.Requests
{
    public class CreateCustomerRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Status { get; set; }

        public ContactDto Contact { get; set; }

    }
}
