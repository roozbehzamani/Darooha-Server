using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darooha.Data.Models
{
    public class Tbl_User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MobPhone { get; set; }

        public string HomePhone { get; set; }

        public string NCode { get; set; }

        [Required]
        public bool EmailConfirm { get; set; }

        [Required]
        public string EmailConfirmCode { get; set; }

        [Required]
        public bool MobPhoneConfirm { get; set; }

        [Required]
        public string MobPhoneConfirmCode { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string ImageURL { get; set; }

        public string ImageID { get; set; }

        public string NotificationCode { get; set; }

        [Required]
        public bool UserEnableStatus { get; set; }

        public bool OnlineStatus { get; set; }

        public DateTime LastOnlineTime { get; set; }


        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Tbl_UserAddress> Tbl_UserAddresses { get; set; }
        public virtual ICollection<Tbl_Wallet> Tbl_Wallets { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Tbl_Order> Tbl_Orders { get; set; }
        public virtual ICollection<Tbl_Comment> Tbl_Comments { get; set; }
    }
}
