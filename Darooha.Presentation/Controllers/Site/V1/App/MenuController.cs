using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.App.Menu;
using Darooha.Data.Models;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.App
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1_Site_App")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuController> _logger;

        public MenuController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper, ILogger<MenuController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(ApiV1Routes.MenuSite.GetMenu)]
        public async Task<IActionResult> GetMenu()
        {
            var getAllMenu = await _db.MenuRepository.GetAllAsync();
            var allMenus = _mapper.Map<IEnumerable<Tbl_Menu>, List<MenuForReturnDto>>(getAllMenu);
            return Ok(allMenus);
        }

        [HttpGet(ApiV1Routes.MenuSite.GetSubMenu)]
        public async Task<IActionResult> GetSubMenu(string id)
        {
            var getAllSubMenus = await _db.SubMenuRepository.GetManyAsync(p => p.MenuId == id, null, "");
            var allSubMenus = _mapper.Map<IEnumerable<Tbl_SubMenu>, List<SubMenuForReturnDto>>(getAllSubMenus);
            return Ok(allSubMenus);
        }
    }
}