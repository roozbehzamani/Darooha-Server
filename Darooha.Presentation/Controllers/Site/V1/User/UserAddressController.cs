using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.UserAddress;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Darooha.Presentation.Controllers.Site.V1.User
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAddressController> _logger;
        public UserAddressController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper, ILogger<UserAddressController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPost(ApiV1Routes.UserAddress.AddUserAddress)]
        public async Task<IActionResult> AddUserAddress(string userId, UserAddressForCreateDTO userAddressForCreateDTO)
        {
            var addressFromRepo = await _db.UserAddressRepository.GetAsync(p => p.Address.Equals(userAddressForCreateDTO.Address) && p.UserId.Equals(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (addressFromRepo == null)
            {
                var addressForCreate = new Tbl_UserAddress()
                {
                    UserId = userId,
                    AddressLatLng = "ندارد"
                };
                var address = _mapper.Map(userAddressForCreateDTO, addressForCreate);
                await _db.UserAddressRepository.InsertAsync(address);

                if (await _db.SaveAsync())
                {
                    return CreatedAtRoute("GetUserAddress", new { id = address.ID, userId = userId }, address);
                }
                else
                {
                    return BadRequest("خطا در ذخیره اطلاعات");
                }
            }
            else
            {
                return BadRequest($"این آدرس قبلا تحت عنوان {userAddressForCreateDTO.AddressName} اضافه شده");
            }
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.UserAddress.GetUserAddress, Name = "GetUserAddress")]
        public async Task<IActionResult> GetUserAddress(string id, string userId)
        {
            var addressFromRepo = await _db.UserAddressRepository.GetByIdAsync(id);
            if (addressFromRepo != null)
            {
                if (addressFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var address = _mapper.Map<UserAddressForReturnDTO>(addressFromRepo);

                    return Ok(address);
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد دسترسی به کارت های شخص دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به کارت های دیگری را ندارید");

                }
            }
            else
            {
                return BadRequest("کارتی با این مشخصات وجود ندارد");
            }
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.UserAddress.GetAllUserAddress)]
        public async Task<IActionResult> GetAllUserAddress(string userId)
        {
            var addresses = await _db.UserAddressRepository.GetManyAsync(p => p.UserId.Equals(userId), null, "");
            var allAddress = _mapper.Map<IEnumerable<Tbl_UserAddress>, List<UserAddressForDetaileDTO>>(addresses);
            return Ok(allAddress);
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPut(ApiV1Routes.UserAddress.UpdateUserAddress)]
        public async Task<IActionResult> UpdateUserAddress(string id, UserAddressForUpdateDTO userAddressForUpdateDTO)
        {
            var addressFromRepo = await _db.UserAddressRepository.GetByIdAsync(id);
            if (addressFromRepo != null)
            {
                if (addressFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var address = _mapper.Map(userAddressForUpdateDTO, addressFromRepo);
                    _db.UserAddressRepository.Update(address);

                    if (await _db.SaveAsync())
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("خطا در بروزرسانی اطلاعات");
                    }
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد بروزرسانی کارت شخص دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به کارت های دیگری را ندارید");

                }
            }
            else
            {
                return BadRequest("کارتی با این مشخصات وجود ندارد");
            }
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpDelete(ApiV1Routes.UserAddress.DeleteUserAddress)]
        public async Task<IActionResult> DeleteUserAddress(string id)
        {
            var addressFromRepo = await _db.UserAddressRepository.GetByIdAsync(id);
            if (addressFromRepo != null)
            {
                if (addressFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    _db.UserAddressRepository.Delete(addressFromRepo);

                    if (await _db.SaveAsync())
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("خطا در حذف اطلاعات");
                    }
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد حذف کارت شخص دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به کارت های دیگری را ندارید");

                }
            }
            else
            {
                return BadRequest("کارتی با این مشخصات وجود ندارد");
            }
        }
    }
}