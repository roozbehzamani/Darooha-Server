using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Notification;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.User
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly ILogger<NotificationsController> _logger;
        private readonly IMapper _mapper;

        public NotificationsController(IUnitOfWork<DaroohaDbContext> dbContext, ILogger<NotificationsController> logger,
            IMapper mapper)
        {
            _db = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPut(ApiV1Routes.Notification.UpdateUserNotify)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> UpdateUserNotify(string userId, NotificationForUpdateDTO notificationForUpdateDto)
        {
            var notifyFromRepo = (await _db.NotificationRepository
                .GetManyAsync(p => p.UserId == userId, null, "")).SingleOrDefault();

            if (notifyFromRepo != null)
            {
                var notifyForUpdate = _mapper.Map(notificationForUpdateDto, notifyFromRepo);

                _db.NotificationRepository.Update(notifyForUpdate);

                if (await _db.SaveAsync())
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("خطای ثبت در دیتابیس");
                }
            }
            else
            {
                var notifyToCreate = new Notification
                {
                    UserId = userId
                };
                var notifyForCreate = _mapper.Map(notificationForUpdateDto, notifyToCreate);
                await _db.NotificationRepository.InsertAsync(notifyForCreate);
                if (await _db.SaveAsync())
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("خطای ثبت در دیتابیس");
                }
            }

        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpGet(ApiV1Routes.Notification.GetUserNotify)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetUserNotify(string userId)
        {
            var notifyFromRepo = (await _db.NotificationRepository
                .GetManyAsync(p => p.UserId == userId, null, "")).SingleOrDefault();

            if (notifyFromRepo != null)
            {
                if (notifyFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    return Ok(notifyFromRepo);
                }
                else
                {
                    _logger.LogError($"کاربر   {userId} قصد دسترسی به اطلاعات notify دیگری را دارد");

                    return Unauthorized($"شما اجازه دسترسی به این اطلاعات را ندارید");

                }
            }
            else
            {

                return BadRequest("اطلاعات اطلاع رسانی وجود ندارد");

            }

        }
    }
}