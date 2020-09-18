using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_Order : BaseEntity<string>
    {
        public Tbl_Order()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public short PaymentMethod { get; set; } //1:wallet 2:online 3:cash
        [Required]
        public short Status { get; set; }
        [Required]
        public int TotalPrice { get; set; }
        //=====================================================
        [Required]
        public string OrderAddress { get; set; }
        //[ForeignKey("AddressId")]
        //public Tbl_UserAddress UserAddresses { get; set; }
        //====================================================
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Tbl_User User { get; set; }
        //==================================================
        public virtual ICollection<Tbl_OrderItem> Tbl_OrderItems { get; set; }
        //public virtual ICollection<Tbl_OrderAddress> Tbl_OrderAddresss { get; set; }
    }
}