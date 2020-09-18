using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.User
{
    public class PasswordForChangeDTO
    {
        [Required(ErrorMessage = "رمز قبلی خود را وارد نمایید")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "رمز جدید خود را وارد نمایید")]
        [MinLength(4, ErrorMessage = "رمز جدید نمیتواند از 4 کاراکتر کمتر باشد")]
        public string NewPassword { get; set; }
    }
}
