﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Entities
{
    public class Note
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required, MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

        [ForeignKey("CustomerID")]
        public Customer? Customer { get; set; }
        public int CustomerID { get; set; }

    }

}
