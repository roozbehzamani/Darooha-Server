﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class TicketContent : BaseEntity<string>
    {
        public TicketContent()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public bool IsAdminSide { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 0)]
        public string Text { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        public string FileUrl { get; set; }

        [Required]
        public string TicketId { get; set; }
        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }
    }
}