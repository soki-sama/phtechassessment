using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Models
{
    public class NoteDto
    {
        public int ID { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }
    }
}
