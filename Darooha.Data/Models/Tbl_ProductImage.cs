using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_ProductImage : BaseEntity<string>
    {
        public Tbl_ProductImage()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string ImageUrl { get; set; }
        //=================================================
        [Required]
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Tbl_Product Tbl_Product { get; set; }
    }
}