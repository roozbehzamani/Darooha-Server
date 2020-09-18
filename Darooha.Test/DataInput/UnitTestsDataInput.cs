using Darooha.Data.Dtos.Common.Token;
using Darooha.Data.Dtos.Services;
using Darooha.Data.Dtos.Site.Panel.Notification;
using Darooha.Data.Dtos.Site.Panel.Photo;
using Darooha.Data.Dtos.Site.Panel.Roles;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Data.Dtos.Site.Panel.UserAddress;
using Darooha.Data.Models;
using Moq;
using System;
using System.Collections.Generic;

namespace Darooha.Test.DataInput
{
    public static class UnitTestsDataInput
    {

        public const string baseRouteV1 = "api/v1/";
        public static readonly string aToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDc2MzU2ZS01NDBhLTQ4ZjAtOGJjNy1kZTRjZmRkZGI0YmIiLCJ1bmlxdWVfbmFtZSI6IjEyMzQ1Njc4OTE0IiwibmJmIjoxNTgzMjY5ODIzLCJleHAiOjE1ODQ1NjU4MjMsImlhdCI6MTU4MzI2OTgyM30.GyN5aR3B9YzjBUex2_7uCl6z-AdxbD3FGed77Up1ryw";

        public static readonly IEnumerable<Role> Roles = new List<Role>()
        {
            new Role()
            {
                Id = "0b83c5e3-404e-44ea-8013-122b6914a",
                Name = "Admin"
            },
            new Role()
            {
                Id = "0b83c5e3-404e-44ea-8013-12253fa",
                Name = "User"
            }
        };
        public static readonly IList<string> RolesString = new List<string>()
        {
           "Admin","Blog"
        };
        public static readonly RoleEditDto roleEditDto = new RoleEditDto
        {
            RoleNames = new[] { "User" }
        };

        public static readonly string userLogedInUsername = "09055365825";
        public static readonly string userLogedInPassword = "password";
        public static readonly string currentUserId = "6928c4c5-657b-43d3-8188-8ef2a4c9da6b";
        public static readonly string userAnOtherId = "799cee6a-b565-4532-b613-f03c5e4c6585";


        public static readonly UserForRegisterDTO userForRegisterDto = new UserForRegisterDTO()
        {
            Email = "asasasasas@barkarama.com",
            Password = "password",
            FirstName = "کیوان",
            LastName = "کیوان",
            MobPhone = "12547896325"
        };
        public static readonly UserForRegisterDTO userForRegisterDto_Fail_Exist = new UserForRegisterDTO()
        {
            Email = "asdfgh@barkarama.com",
            Password = "password",
            FirstName = "vbnjhg",
            LastName = "vcddfg",
            MobPhone = userLogedInUsername
        };
        public static readonly UserForRegisterDTO userForRegisterDto_Fail_ModelState = new UserForRegisterDTO()
        {
            Email = "",
            Password = "",
            FirstName = "",
            LastName = "",
            MobPhone = ""
        };


        public static readonly TokenRequestDTO useForLoginDto_Success_password = new TokenRequestDTO()
        {
            GrantType = "password",
            UserName = "09055365825",
            Password = "password",
            IsRemember = true
        };
        public static readonly TokenRequestDTO useForLoginDto_Success_refreshToken = new TokenRequestDTO()
        {
            GrantType = "refresh_token",
            UserName = "09055365825",
            RefreshToken = "b4f7dfdfa5454674a6d197257142c498",
            IsRemember = true
        };
        public static readonly TokenRequestDTO useForLoginDto_Fail_refreshToken = new TokenRequestDTO()
        {
            GrantType = "refresh_token",
            UserName = "09055365825",
            RefreshToken = "noRefreshToken",
            IsRemember = true
        };
        public static readonly TokenRequestDTO useForLoginDto_Fail_password = new TokenRequestDTO()
        {
            GrantType = "password",
            UserName = "00@000.com",
            Password = "password",
            IsRemember = true
        };
        public static readonly TokenRequestDTO useForLoginDto_Fail_ModelState = new TokenRequestDTO()
        {

            UserName = string.Empty,
            GrantType = string.Empty
        };


        public static readonly PhotoForUserProfileDTO photoForProfileDto = new PhotoForUserProfileDTO
        {
            ImageURL = "http://google.com",
            ImageID = "1"
        };


        public static readonly UserForUpdateDTO userForUpdateDto = new UserForUpdateDTO
        {
            FirstName = "روزبه",
            LastName = "زمانی",
            MobPhone = "09055365825",
            Email = "zazaza@gmaiiil.com"
        };
        public static readonly UserForUpdateDTO userForUpdateDto_Fail_ModelState = new UserForUpdateDTO
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            MobPhone = string.Empty,
            Email = string.Empty
        };


        public static readonly PasswordForChangeDTO passwordForChangeDto = new PasswordForChangeDTO()
        {
            OldPassword = It.IsAny<string>(),
            NewPassword = It.IsAny<string>()
        };
        public static readonly PasswordForChangeDTO passwordForChangeDto_Fail = new PasswordForChangeDTO()
        {
            OldPassword = "123789",
            NewPassword = "123789"
        };
        public static readonly PasswordForChangeDTO passwordForChangeDto_Fail_ModelState = new PasswordForChangeDTO()
        {
            OldPassword = string.Empty,
            NewPassword = string.Empty
        };

        public static readonly PhotoForReturnProfileDTO PhotoForReturnProfileDto = new PhotoForReturnProfileDTO()
        { };
        public static readonly FileUploadedDTO fileUploadedDto_Success = new FileUploadedDTO()
        {
            Status = true,
            Message = "با موفقیت در لوکال آپلود شد",
            PublicID = "0",
            Url = "wwwroot/Files/Pic/Profile/"
        };
        public static readonly FileUploadedDTO fileUploadedDto_Fail_WrongFile = new FileUploadedDTO()
        {
            Status = false,
            Message = "فایلی برای اپلود یافت نشد"
        };

        public static readonly IEnumerable<Tbl_User> GetUser = new List<Tbl_User>()
        {
            new Tbl_User
            {
                Id = "6928c4c5-657b-43d3-8188-8ef2a4c9da6b",
                FirstName = "روزبه",
                LastName = "زمانی",
                MobPhone = "09055365825",
                HomePhone = null,
                NCode = null,
                Email = "zazaza@gmaiiil.com",
                EmailConfirm = true,
                EmailConfirmCode = "1741034681",
                MobPhoneConfirm = true,
                MobPhoneConfirmCode = "1741034681",
                Password = "1744",
                BirthDate = DateTime.Now,
                Gender = null,
                ImageURL = "http://res.cloudinary.com/Darooha/image/upload/v1583617133/profile/nycdmmoivgkjisz09fvo.bmp",
                ImageID = "profile/nycdmmoivgkjisz09fvo",
                NotificationCode = null,
                UserEnableStatus = true,
                OnlineStatus = false,
                LastOnlineTime = DateTime.Now,
                Tbl_UserAddresses = new List<Tbl_UserAddress>()
                {
                    new Tbl_UserAddress()
                    {
                        ID = "4476356e-540a-50f0-8bc7-de4cfdddb4bb",
                        UserId = "6928c4c5-657b-43d3-8188-8ef2a4c9da6b",
                        Address = "لبسیلسیلسیبلسیب",
                        AddressLatLng = "سیبسیلسیبسیب",
                        AddressName = "سیبسیبسیبسییبسیب",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    }
                },
                Tickets = new List<Ticket>()
                {
                    new Ticket()
                    {
                        ID = "0b83c5e3-404e-44ea91453fa",
                        UserId = "6928c4c5-657b-43d3-8188-8ef2a4c9da6b",
                        TicketContents = new List<TicketContent>()
                        {
                            new TicketContent()
                            {
                                TicketId = "0b83c5e3-404e-44ea91453fa"
                            }
                        }
                    }
                }
            }
        };
        public static UserForDetailedDTO GetUserForDetailedDto()
        {
            return new UserForDetailedDTO()
            {
                ID = "4476356e-540a-48f0-8bc7-de4cfdddb4bb",
                FirstName = "روزبه",
                LastName = "زمانی",
                MobPhone = "09055365825",
                Email = "zazaza@gmaiiil.com",
                ImageURL = "http://res.cloudinary.com/Darooha/image/upload/v1583617133/profile/nycdmmoivgkjisz09fvo.bmp"
            };
        }

        public static readonly Tbl_Setting settingForUpload = new Tbl_Setting()
        {
            CloudinaryCloudName = "12",
            CloudinaryAPIkey = "12",
            CloudinaryAPISecret = "12"
        };

        public static readonly IEnumerable<Notification> notify_Success = new List<Notification>()
        {
            new Notification()
            {
                UserId = "0b83c5e3-404e-44ea-8013-122b691453fa",
            EnterEmail = true,
            EnterSms = false,
            EnterTelegram = true,
            ExitEmail = true,
            ExitSms = false,
            ExitTelegram = true,
            LoginEmail = true,
            LoginSms = false,
            LoginTelegram = true,
            TicketEmail = true,
            TicketSms = false,
            TicketTelegram = true
            }

        };
        public static readonly NotificationForUpdateDTO notifyForUpdate_Success = new NotificationForUpdateDTO()
        {
            EnterEmail = true,
            EnterSms = false,
            EnterTelegram = true,
            ExitEmail = true,
            ExitSms = false,
            ExitTelegram = true,
            LoginEmail = true,
            LoginSms = false,
            LoginTelegram = true,
            TicketEmail = true,
            TicketSms = false,
            TicketTelegram = true

        };

        public static readonly UserAddressForReturnDTO userAddressForReturnDTO_Success = new UserAddressForReturnDTO()
        {
            Address = "لبسیلسیلسیبلسیب",
            AddressLatLng = "سیبسیلسیبسیب",
            AddressName = "سیبسیبسیبسییبسیب"
        };
        public static readonly UserAddressForCreateDTO userAddressForCreateDTO_Success = new UserAddressForCreateDTO()
        {
            Address = "لبسیلسیلسیبلسیب",
            AddressName = "سیبسیبسیبسییبسیب"
        };


    }
}
