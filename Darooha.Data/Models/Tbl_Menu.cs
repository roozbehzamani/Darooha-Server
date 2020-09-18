using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darooha.Data.Models
{
    public class Tbl_Menu : BaseEntity<string>
    {
        public Tbl_Menu()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string MenuName { get; set; }
        [Required]
        public string MenuImage { get; set; }
        //=============================================================================
        public virtual ICollection<Tbl_SubMenu> Tbl_SubMenus { get; set; }
    }
}
