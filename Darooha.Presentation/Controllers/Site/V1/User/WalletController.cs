using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Wallet;
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
    public class WalletController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly ILogger<WalletController> _logger;
        private readonly IMapper _mapper;

        public WalletController(IUnitOfWork<DaroohaDbContext> dbContext, ILogger<WalletController> logger,
            IMapper mapper)
        {
            _db = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPut(ApiV1Routes.Wallet.AddToWallet)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddToWallet(string userId, WalletInventoryForUpdateDTO walletInventoryForUpdateDTO)
        {
            var walletFromRepo = (await _db.WalletRepository
                .GetManyAsync(p => p.UserId == userId, null, "")).SingleOrDefault();

            if (walletFromRepo != null)
            {
                walletFromRepo.Inventory = walletFromRepo.Inventory + walletInventoryForUpdateDTO.Inventory;

                _db.WalletRepository.Update(walletFromRepo);

                if (await _db.SaveAsync())
                {
                    var returnWallet = _mapper.Map<WalletForReturnDTO>(walletFromRepo);
                    return Ok(returnWallet);
                }
                else
                {
                    return BadRequest("خطای ثبت در دیتابیس");
                }
            }
            else
            {
                var walletToCreate = new Tbl_Wallet
                {
                    UserId = userId,
                    Inventory = walletInventoryForUpdateDTO.Inventory
                };
                await _db.WalletRepository.InsertAsync(walletToCreate);
                if (await _db.SaveAsync())
                {
                    var returnWallet = _mapper.Map<WalletForReturnDTO>(walletToCreate);
                    return Ok(returnWallet);
                }
                else
                {
                    return BadRequest("خطای ثبت در دیتابیس");
                }
            }

        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPost(ApiV1Routes.Wallet.GetWallet)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetWallet(string userId)
        {
            var walletFromRepo = (await _db.WalletRepository
                .GetManyAsync(p => p.UserId == userId, null, "")).SingleOrDefault();

            if (walletFromRepo != null)
            {
                if (walletFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var returnWallet = _mapper.Map<WalletForReturnDTO>(walletFromRepo);
                    return Ok(returnWallet);
                }
                else
                {
                    _logger.LogError($"کاربر   {userId} قصد دسترسی به اطلاعات کیف پول دیگری را دارد");
                    return Unauthorized($"شما اجازه دسترسی به این اطلاعات را ندارید");
                }
            }
            else
            {
                var walletForCreate = new Tbl_Wallet()
                {
                    UserId = userId,
                    Inventory = 0
                };
                await _db.WalletRepository.InsertAsync(walletForCreate);

                if (await _db.SaveAsync())
                {
                    var returnWallet = _mapper.Map<WalletForReturnDTO>(walletForCreate);
                    return Ok(returnWallet);
                }
                else
                {
                    return BadRequest("خطا در ذخیره اطلاعات");
                }
            }

        }
    }
}