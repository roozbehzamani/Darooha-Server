using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.App.Menu;
using Darooha.Data.Dtos.Site.App.Product;
using Darooha.Data.Dtos.Site.App.Slider;
using Darooha.Data.Models;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.App
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1_Site_App")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        public HomeController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
        }

        [HttpGet(ApiV1Routes.Home.GetNewProduct)]
        public async Task<IActionResult> GetNewProduct()
        {
            var getFourProduct = (await _db.ProductRepository.GetAllAsync()).OrderByDescending(x => x.DateCreated).Take(4);
            var allFourProduct = _mapper.Map<IEnumerable<Tbl_Product>, List<ProductForReturnDto>>(getFourProduct);
            foreach (var item in allFourProduct)
            {
                item.ImageUrl = (await _db.ProductImageRepository.GetManyAsync(p => p.ProductId == item.ID, null, "")).FirstOrDefault().ImageUrl;
            }
            return Ok(allFourProduct);
        }

        [HttpGet(ApiV1Routes.Home.GetSliderItems)]
        public async Task<IActionResult> GetSliderItems()
        {
            var getAllSliderItem = await _db.HomeFirstSliderRepository.GetAllAsync();
            var allItems = _mapper.Map<IEnumerable<Tbl_HomeFirstSlider>, List<SliderItemToReturnDto>>(getAllSliderItem);
            return Ok(allItems);
        }

        [HttpGet(ApiV1Routes.Home.GetSingleProduct)]
        public async Task<IActionResult> GetSingleProduct()
        {
            var getProduct = (await _db.ProductRepository.GetManyAsync(p => p.IsSpecial == true, null, "")).FirstOrDefault();
            var product = _mapper.Map<Tbl_Product, SpecialProductForReturnDto>(getProduct);
            product.FirstImageUrl = (await _db.ProductImageRepository.GetManyAsync(p => p.ProductId == product.ID, null, "")).FirstOrDefault().ImageUrl;
            product.SecendImageUrl = (await _db.ProductImageRepository.GetManyAsync(p => p.ProductId == product.ID, null, "")).Skip(1).Take(1).LastOrDefault().ImageUrl;
            return Ok(product);
        }

        [HttpGet(ApiV1Routes.Home.GetNewMenus)]
        public async Task<IActionResult> GetNewMenus()
        {
            var getMenu = (await _db.MenuRepository.GetAllAsync()).OrderByDescending(x => x.DateCreated).Take(6);
            var allItems = _mapper.Map<IEnumerable<Tbl_Menu>, List<NewMenuForReturnDto>>(getMenu);
            return Ok(allItems);
        }
    }
}