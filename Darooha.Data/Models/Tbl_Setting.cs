using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Darooha.Data.Models
{
    public class Tbl_Setting : BaseEntity<int>
    {
        public Tbl_Setting()
        {
            ID = new int();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Column(TypeName = "text")]
        public string CompanyAddress { get; set; }

        [StringLength(50)]
        public string FirstPhone { get; set; }

        [StringLength(50)]
        public string SecendPhone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(150)]
        public string TelegramID { get; set; }

        [StringLength(150)]
        public string InstagramChannel { get; set; }

        [Column(TypeName = "text")]
        public string AboutUs { get; set; }

        [StringLength(100)]
        public string AppLogo { get; set; }

        [StringLength(100)]
        public string ENamadLogo { get; set; }

        [StringLength(100)]
        public string CompanyLogo { get; set; }

        [StringLength(100)]
        public string Smtp { get; set; }

        [StringLength(100)]
        public string FromEmail { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string SmsPanelPassword { get; set; }

        [StringLength(100)]
        public string Sender { get; set; }

        public bool? RamesanCloseOrder { get; set; }

        public bool? CommentShowWithConfirm { get; set; }

        [StringLength(50)]
        public string StudentDinnerEndTime { get; set; }

        [StringLength(50)]
        public string AppWorkingTime { get; set; }

        [StringLength(50)]
        public string CharityName { get; set; }

        [Column(TypeName = "text")]
        public string RamesanMessage { get; set; }

        public bool? EnableRamesanMessage { get; set; }

        [Required]
        public string CloudinaryCloudName { get; set; }

        [Required]
        public string CloudinaryAPIkey { get; set; }

        [Required]
        public string CloudinaryAPISecret { get; set; }
        [Required]
        public bool UploadLocal { get; set; }
    }
}
