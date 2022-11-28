﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Propeller.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        // public int StatusID { get; set; }

        [ForeignKey("CustomerStatusID")]
        public CustomerStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModified { get; set; }

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();

    }
}