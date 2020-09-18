using AutoMapper;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Photo;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Upload.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Darooha.Presentation.Controllers.Site.V1.User
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PhotosController> _logger;

        public PhotosController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper, IUploadService uploadService, IWebHostEnvironment env, ILogger<PhotosController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _uploadService = uploadService;
            _env = env;
            _logger = logger;
        }

        [Authorize(Policy = "AccessProfile")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpPost(ApiV1Routes.Photos.ChangeUserPhoto)]
        public async Task<IActionResult> ChangeUserPhoto(string userId, [FromForm]PhotoForUserProfileDTO photoForUserProfileDTO)
        {
            var userFromRepo = await _db.UserRepository.GetByIdAsync(userId);
            string baseUrl = string.Format("{0}://{1}{2}", Request.Scheme ?? "", Request.Host.Value ?? "", Request.PathBase.Value ?? "");
            var uploadRes = await _uploadService.UploadProfilePic(photoForUserProfileDTO.File, userFromRepo.Id, _env.WebRootPath, baseUrl, userFromRepo.ImageID);

            if (uploadRes.Status)
            {
                userFromRepo.ImageURL = uploadRes.Url;
                userFromRepo.ImageID = uploadRes.PublicID;

                _db.UserRepository.Update(userFromRepo);
                if (await _db.SaveAsync())
                {
                    var photoForReturn = _mapper.Map<PhotoForReturnProfileDTO>(userFromRepo);
                    return CreatedAtRoute("GetPhoto", new { id = userFromRepo.Id }, photoForReturn);
                }
                else
                {
                    return BadRequest(new ReturnErrorMessage()
                    {
                        Status = false,
                        Title = "خطا",
                        Message = "آپلود عکس خطا دارد . لطفا مجددا تلاش نمایید"
                    });
                }
            }
            else
            {
                return BadRequest(new ReturnErrorMessage()
                {
                    Status = false,
                    Title = "خطا",
                    Message = uploadRes.Message
                });
            }
        }

        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Photos.GetPhoto, Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(string id)
        {

            var photoFromRepo = await _db.UserRepository.GetByIdAsync(id);
            if (photoFromRepo != null)
            {
                if (photoFromRepo.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var photo = _mapper.Map<PhotoForReturnProfileDTO>(photoFromRepo);

                    return Ok(photo);
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد دسترسی به عکس شخص دیگری را دارد");

                    return BadRequest(new ReturnErrorMessage()
                    {
                        Status = false,
                        Title = "خطا",
                        Message = $"شما اجازه دسترسی به عکس کاربر دیگری را ندارید"
                    });

                }
            }
            else
            {
                return BadRequest("عکسی وجود ندارد");
            }
        }
    }
}