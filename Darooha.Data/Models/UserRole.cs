using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public Tbl_User User { get; set; }
        public Role Role { get; set; }
    }
}
