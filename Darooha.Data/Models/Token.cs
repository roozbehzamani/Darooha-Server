using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darooha.Data.Models
{
    public class Token : BaseEntity<string>
    {
        public Token()
        {
            ID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Tbl_User User { get; set; }
        [Required]
        public string Ip { get; set; }
    }
}
