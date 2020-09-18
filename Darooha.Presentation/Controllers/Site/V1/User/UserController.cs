using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Services.Site.Admin.User.Interface;
using Microsoft.Extensions.Logging;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Microsoft.AspNetCore.Authorization;

namespace Darooha.Presentation.Controllers.Site.V1.User
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper, IUserService userservice, ILogger<UserController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _userService = userservice;
            _logger = logger;
        }

        [Authorize(Policy = "AccessProfile")]
        [HttpGet(ApiV1Routes.Users.GetUser, Name = "GetUser")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _db.UserRepository.GetByIdAsync(id);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }

        [Authorize(Policy = "AccessProfile")]
        [HttpPut(ApiV1Routes.Users.UpdateUser)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdateDTO userForUpdateDTO)
        {
            var userFromRepo = await _db.UserRepository.GetByIdAsync(id);
            _mapper.Map(userForUpdateDTO, userFromRepo);
            _db.UserRepository.Update(userFromRepo);
            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                _logger.LogError($"آپدیت کاربر {id} با خطا مواجه شد");
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = "آپدیت با خطا مواجه شد"
                });
            }
        }

        [Authorize(Policy = "AccessProfile")]
        [HttpPut(ApiV1Routes.Users.ChangeUserPassword)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> ChangeUserPassword(string id, PasswordForChangeDTO passwordForChangeDto)
        {
            var userFromRepo = await _userService.GetUserForPassChange(id, passwordForChangeDto.OldPassword);
            if (userFromRepo == null)
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = "پسورد فعلی اشتباه میباشد . لطفا مجددا تلاش نمایید"
                });
            if (await _userService.UpdateUserPassword(userFromRepo, passwordForChangeDto.NewPassword))
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = "ویرایش انجام نشد"
                });
            }
        }
    }
}