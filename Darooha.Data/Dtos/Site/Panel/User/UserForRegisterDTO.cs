using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.User
{
    public class UserForRegisterDTO
    {
        [Required(ErrorMessage ="رمز ورود خود را وارد نمایید")]
        [MinLength(4, ErrorMessage = "رمز نمیتواند از 4 کاراکتر کمتر باشد")]
        public string Password { get; set; }
        [Required(ErrorMessage ="نام خود را وارد نمایید")]
        [MinLength(3 , ErrorMessage ="نام نمیتواند از سه حرف کمتر باشد")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="نام خانوادگی خود را وارد نمایید")]
        [MinLength(3, ErrorMessage = "نام خانوادگی نمیتواند از سه حرف کمتر باشد")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="شماره تلفن همراه خود را وارد نمایید")]
        [Phone(ErrorMessage ="شماره تلفن وارد شده صحیح نمیباشد")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره تلفن باید 11 رقمی باشد (با در نظر گرفتن 0 وارد نمایید)")]
        public string MobPhone { get; set; }
        [Required(ErrorMessage ="ایمیل خود را وارد نمایید")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده صحیح نمیباشد")]
        public string Email { get; set; }
    }
}
