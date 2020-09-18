using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_Wallet : BaseEntity<string>
    {
        public Tbl_Wallet()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public int Inventory { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Tbl_User User { get; set; }
    }
}
