using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Common.Token;
using Darooha.Data.Dtos.Site.Panel.Auth;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Data.Models;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.Auth.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.Auth
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IUtilities _utilities;
        private readonly UserManager<Tbl_User> _userManager;

        public AuthController(IAuthService authService, IMapper mapper, ILogger<AuthController> logger, IUtilities utilities,
            UserManager<Tbl_User> userManager)
        {
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost(ApiV1Routes.Auth.Register)]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            var userToCreate = new Tbl_User()
            {
                Email = userForRegisterDTO.Email,
                EmailConfirm = false,
                EmailConfirmCode = "1741034681",
                FirstName = userForRegisterDTO.FirstName,
                LastName = userForRegisterDTO.LastName,
                MobPhone = userForRegisterDTO.MobPhone,
                MobPhoneConfirm = false,
                MobPhoneConfirmCode = "1741034681",
                OnlineStatus = false,
                Password = userForRegisterDTO.Password,
                UserEnableStatus = false,
                UserName = userForRegisterDTO.MobPhone,
                ImageURL = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host.Value ?? "", Request.PathBase.Value ?? "", "wwwroot/Files/Pic/default-profile.png")
            };

            var notifyToCreate = new Notification
            {
                UserId = userToCreate.Id,
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

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDTO.Password);

            if (result.Succeeded)
            {
                await _authService.AddUserPreNeededAsync(notifyToCreate);
                var userForReturn = _mapper.Map<UserForDetailedDTO>(userToCreate);

                return CreatedAtRoute("GetUser", new
                {
                    controller = "User",
                    id = userToCreate.Id
                }, userForReturn);
            }
            else if (result.Errors.Any())
            {
                _logger.LogWarning(result.Errors.First().Description);
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = result.Errors.First().Description
                });
            }
            else
            {
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = "خطای ثبت در پایگاه داده"
                });
            }
        }

        [HttpPost(ApiV1Routes.Auth.Login)]
        public async Task<IActionResult> Login(TokenRequestDTO tokenRequestDTO)
        {
            switch (tokenRequestDTO.GrantType)
            {
                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenRequestDTO);
                    if (result.status)
                    {
                        var userForReturn = _mapper.Map<UserForDetailedDTO>(result.user);

                        return Ok(new LoginResponseDTO
                        {
                            token = result.token,
                            refresh_token = result.refresh_token,
                            user = userForReturn
                        });
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDTO.UserName} درخواست لاگین ناموفق داشته است" + "---" + result.message);
                        return Unauthorized("1x111keyvanx11");
                    }
                case "refresh_token":
                    var res = await _utilities.RefreshAccessTokenAsync(tokenRequestDTO);
                    if (res.status)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDTO.UserName} درخواست لاگین ناموفق داشته است" + "---" + res.message);
                        return Unauthorized("0x000keyvanx00");
                    }
                default:
                    return Unauthorized("خطا در اعتبار سنجی دوباره");
            }
        }
    }
}