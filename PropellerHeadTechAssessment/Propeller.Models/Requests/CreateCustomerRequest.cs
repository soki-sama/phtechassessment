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
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        public ContactDto? Contact { get; set; }

    }
}
