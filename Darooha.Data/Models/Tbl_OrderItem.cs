using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_OrderItem : BaseEntity<string>
    {
        public Tbl_OrderItem()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public int ItemCount { get; set; }
        //==============================================================
        [Required]
        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Tbl_Order Tbl_Order { get; set; }
        //==============================================================
        [Required]
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Tbl_Product Tbl_Product { get; set; }
    }
}