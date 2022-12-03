using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Models
{
    public class CountryDto
    {
        [Required]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string CountryCode { get; set; } = string.Empty;

        [Required]
        public string DefaultLocale { get; set; } = string.Empty;

    }
}
