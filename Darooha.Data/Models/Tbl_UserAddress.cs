using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_UserAddress : BaseEntity<string>
    {
        public Tbl_UserAddress()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string AddressName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string AddressLatLng { get; set; }
        //===========================================================
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Tbl_User User { get; set; }
        //=========================================================
        //public ICollection<Tbl_Order> Tbl_Orders { get; set; }
        //public virtual ICollection<Tbl_OrderAddress> Tbl_OrderAddresss { get; set; }
    }
}
