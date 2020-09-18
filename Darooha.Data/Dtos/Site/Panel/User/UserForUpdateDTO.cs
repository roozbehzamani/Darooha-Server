using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.User
{
    public class UserForUpdateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MobPhone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
