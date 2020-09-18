using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Roles;
using Darooha.Data.Models;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.Admin
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly DaroohaDbContext _dbDarooha;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminUsersController> _logger;
        private readonly UserManager<Tbl_User> _userManager;


        public AdminUsersController(IUnitOfWork<DaroohaDbContext> dbContext, DaroohaDbContext dbDarooha,
            IMapper mapper,
            UserManager<Tbl_User> userManager, ILogger<AdminUsersController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _dbDarooha = dbDarooha;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(ApiV1Routes.AdminUsers.GetUsers)]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await (from user in _dbDarooha.Users
                               orderby user.UserName
                               select new
                               {
                                   Id = user.Id,
                                   UserName = user.UserName,
                                   Roles = (from userRole in user.UserRoles
                                            join role in _dbDarooha.Roles
                                                on userRole.RoleId
                                                equals role.Id
                                            select role.Name)
                               }).ToListAsync();
            //await _db.UserRepository.GetManyAsync(null, null, "Photos,BankCards");
            // var usersToReturn = _mapper.Map<IEnumerable<UserFroListDto>>(users);

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost(ApiV1Routes.AdminUsers.EditRoles)]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles ??= new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                return BadRequest("خطا در اضافه کردن نقش ها");
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                return BadRequest("خطا در پاک کردن نقش ها");
            }

            return Ok(await _userManager.GetRolesAsync(user));

        }

    }
}