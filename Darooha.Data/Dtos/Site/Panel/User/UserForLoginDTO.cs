using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.User
{
    public class UserForLoginDTO
    {
        [Required(ErrorMessage = "نام کاربری خود را وارد نمایید")]
        [MinLength(4, ErrorMessage = "نام کاربری نمیتواند از 4 کاراکتر کمتر باشد")]
        public string Username { get; set; }
        [Required(ErrorMessage = "رمز ورود خود را وارد نمایید")]
        [MinLength(4, ErrorMessage = "رمز نمیتواند از 4 کاراکتر کمتر باشد")]
        public string Password { get; set; }
        public bool isRemember { get; set; }
    }
}
