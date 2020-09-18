using Darooha.Data.Models;
using Darooha.Services.Seed.Interface;
using Darooha.Services.Site.Admin.Auth.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Darooha.Services.Seed.Service
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<Tbl_User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthService _authService;

        public SeedService(UserManager<Tbl_User> userManager, RoleManager<Role> roleManager, IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("wwwroot/Files/Json/Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<IList<Tbl_User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name="Admin"},
                    new Role{Name="User"},
                    new Role{Name="Blog"},
                    new Role{Name="Accountant"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }

                //Create AdminUser
                var adminUser = new Tbl_User
                {
                    Email = "admin@madpay724.com",
                    FirstName = "roozbeh",
                    LastName = "zamani",
                    MobPhone = "09055365825",
                    EmailConfirmCode = "3947",
                    MobPhoneConfirmCode = "3130",
                    EmailConfirm = false,
                    UserEnableStatus = false,
                    MobPhoneConfirm = true,
                    BirthDate = DateTime.Now,
                    Password = "123456",
                    UserName = "admin@madpay724.com"
                };
                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var notifyToCreate = new Notification
                    {
                        UserId = adminUser.Id,
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

                    _authService.AddUserPreNeededAsync(notifyToCreate).Wait();

                    var admin = _userManager.FindByNameAsync("admin@madpay724.com").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Blog", "Accountant" }).Wait();
                }
                //Create BlogUser
                var blogUser = new Tbl_User
                {
                    Email = "blog@madpay724.com",
                    FirstName = "roozbeh",
                    LastName = "zamani",
                    MobPhone = "09050411917",
                    EmailConfirmCode = "3947",
                    MobPhoneConfirmCode = "3130",
                    EmailConfirm = false,
                    UserEnableStatus = false,
                    MobPhoneConfirm = true,
                    BirthDate = DateTime.Now,
                    Password = "123456",
                    UserName = "blog@madpay724.com"
                };
                IdentityResult resultBlog = _userManager.CreateAsync(blogUser, "password").Result;

                if (resultBlog.Succeeded)
                {
                    var notifyToCreate = new Notification
                    {
                        UserId = adminUser.Id,
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

                    _authService.AddUserPreNeededAsync(notifyToCreate).Wait();

                    var blog = _userManager.FindByNameAsync("blog@madpay724.com").Result;
                    _userManager.AddToRoleAsync(blog, "Blog").Wait();
                }
                //Create AccountantUser
                var accountantUser = new Tbl_User
                {
                    Email = "accountant@madpay724.com",
                    FirstName = "roozbeh",
                    LastName = "zamani",
                    MobPhone = "09143556683",
                    EmailConfirmCode = "3947",
                    MobPhoneConfirmCode = "3130",
                    EmailConfirm = false,
                    UserEnableStatus = false,
                    MobPhoneConfirm = true,
                    BirthDate = DateTime.Now,
                    Password = "123456",
                    UserName = "accountant@madpay724.com",
                };
                IdentityResult resultAccountant = _userManager.CreateAsync(accountantUser, "password").Result;

                if (resultAccountant.Succeeded)
                {
                    var notifyToCreate = new Notification
                    {
                        UserId = adminUser.Id,
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

                    _authService.AddUserPreNeededAsync(notifyToCreate).Wait();

                    var accountant = _userManager.FindByNameAsync("accountant@madpay724.com").Result;
                    _userManager.AddToRoleAsync(accountant, "Accountant").Wait();
                }
            }
        }
    }
}
