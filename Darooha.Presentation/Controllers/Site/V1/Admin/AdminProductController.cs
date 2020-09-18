using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Product;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darooha.Presentation.Controllers.Site.V1.Admin
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;


        public AdminProductController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.AdminProduct.GetAdminProduct, Name = "GetAdminProduct")]
        public async Task<IActionResult> GetAdminProduct(string id, string userId)
        {
            var product = await _db.ProductRepository.GetByIdAsync(id);
            if (product != null)
            {
                var productForReturn = _mapper.Map<AdminProductForReturnDto>(product);

                return Ok(productForReturn);
            }
            else
            {
                return BadRequest("چنبن محصولی وجود ندارد");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.AdminProduct.AllProductList)]
        public async Task<IActionResult> AllProductList(string userId)
        {
            var productList = await _db.ProductRepository.GetAllAsync();
            var productListForReturn = _mapper.Map<IEnumerable<Tbl_Product>, List<AdminProductListForReturnDto>>(productList);
            return Ok(productListForReturn);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpPut(ApiV1Routes.AdminProduct.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(string id, string userId, AdminProductForUpdateDto adminProductForUpdateDto)
        {
            var productFromRepo = await _db.ProductRepository.GetByIdAsync(id);
            if (productFromRepo != null)
            {
                var product = _mapper.Map(adminProductForUpdateDto, productFromRepo);
                _db.ProductRepository.Update(product);

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
                return BadRequest("چنین محصولی وجود ندارد");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpDelete(ApiV1Routes.AdminProduct.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(string id, string userId)
        {
            var productFromRepo = await _db.ProductRepository.GetByIdAsync(id);
            if (productFromRepo != null)
            {
                _db.ProductRepository.Delete(productFromRepo);

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
                return BadRequest("چنین محصولی وجود ندارد");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpPost(ApiV1Routes.AdminProduct.AddProduct)]
        public async Task<IActionResult> AddProduct(AdminProductForCreateDto adminProductForCreateDto, string userId)
        {
            var productFromRepo = await _db.ProductRepository.GetAsync(p => p.ProductName.Equals(adminProductForCreateDto.ProductName));
            if (productFromRepo == null)
            {
                var adminProductForCreate = new Tbl_Product()
                {
                    CommentCount = 0,
                    SumPoint = 0
                };
                var product = _mapper.Map(adminProductForCreateDto, adminProductForCreate);
                await _db.ProductRepository.InsertAsync(product);

                if (await _db.SaveAsync())
                {
                    return CreatedAtRoute("GetAdminProduct", new { id = product.ID }, product);
                }
                else
                {
                    return BadRequest("خطا در ذخیره اطلاعات");
                }
            }
            else
            {
                return BadRequest("این محصول قبلا اضافه شده");
            }
        }

    }
}