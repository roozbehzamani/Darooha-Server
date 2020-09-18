using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_SubMenu : BaseEntity<string>
    {
        public Tbl_SubMenu()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string subMenuName { get; set; }
        //===========================================================
        [Required]
        public string MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Tbl_Menu Tbl_Menu { get; set; }
        //===========================================================
        public virtual ICollection<Tbl_Product> Tbl_Products { get; set; }
    }
}
