using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_Product : BaseEntity<string>
    {
        public Tbl_Product()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string ProductName { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        [Required]
        public int ProductCount { get; set; }
        [Required]
        public bool IsEnable { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public short GenderOfConsumer { get; set; } //true: male false: female
        [Required]
        public string EnclosureType { get; set; } //نوع محفظه
        [Required]
        public string ManufacturingCountry { get; set; }
        [Required]
        public string ManufacturerCompany { get; set; }
        [Required]
        public string WebAddress { get; set; }
        [Required]
        public string Features { get; set; }
        [Required]
        public string MethodUse { get; set; }
        [Required]
        public string Indications { get; set; }
        [Required]
        public string Warnings { get; set; }
        [Required]
        public string Maintenance { get; set; }
        [Required]
        public bool IsSpecial { get; set; }
        public string ScientificName { get; set; }
        public int SumPoint { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        //=========================================================
        public virtual ICollection<Tbl_ProductImage> Tbl_ProductImages { get; set; }
        public virtual ICollection<Tbl_OrderItem> Tbl_OrderItems { get; set; }
        public virtual ICollection<Tbl_HomeFirstSlider> Tbl_HomeFirstSlider { get; set; }
        public virtual ICollection<Tbl_Comment> Tbl_Comments { get; set; }
        //==============================================================
        //[Required]
        //public string SubMenuId { get; set; }
        //[ForeignKey("SubMenuId")]
        public virtual Tbl_SubMenu Tbl_SubMenu { get; set; }
    }
}