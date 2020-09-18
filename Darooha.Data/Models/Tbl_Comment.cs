using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Darooha.Data.Models
{
    public class Tbl_Comment : BaseEntity<string>
    {
        public Tbl_Comment()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string CommentText { get; set; }
        [Required]
        public int CommentPoint { get; set; }
        [Required]
        public bool commentConfirm { get; set; }
        //==================================================
        [Required]
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Tbl_Product Tbl_Product { get; set; }
        //==================================================
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Tbl_User Tbl_User { get; set; }
        //==================================================
        public string CommentResponse { get; set; }
    }
}
