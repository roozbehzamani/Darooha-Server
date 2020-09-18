using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Order;
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
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper, ILogger<OrderController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
        }


        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Order.GetOrders)]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var ordersFromRepo = await _db.OrderRepository.GetManyAsync(p => p.UserId == userId, null, "");
            
            var allOrders = _mapper.Map<IEnumerable<Tbl_Order>, List<OrderToReturnDto>>(ordersFromRepo);
            foreach (var order in allOrders)
            {
                order.userAddress = (await _db.UserAddressRepository.GetManyAsync(p => p.ID == order.AddressID, null, "")).SingleOrDefault().Address;
            }
           

            return Ok(allOrders);
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Order.GetOrder, Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(string id, string userId)
        {
            var orderFromRepo = (await _db.OrderRepository.GetManyAsync(p => p.ID == id, null, "Tbl_OrderItems"))
                .SingleOrDefault();
            if (orderFromRepo != null)
            {
                if (orderFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    orderFromRepo.Tbl_OrderItems.OrderByDescending(p => p.DateCreated);
                    return Ok(orderFromRepo);
                }
                else
                {
                    _logger.LogError($"کاربر   {userId} قصد دسترسی به سفارشات دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به سفارشات کاربر دیگری را ندارید");
                }
            }
            else
            {
                return BadRequest("تیکتی وجود ندارد");
            }

        }

        //--------------------------------------------------------------------------------------------------------------------------------

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Order.GetOrderItems)]
        public async Task<IActionResult> GetOrderItems(string id, string userId)
        {
            var orderFromRepo = await _db.OrderItemRepository.GetManyAsync(p => p.OrderId == id, null, "");
            var allItems = _mapper.Map<IEnumerable<Tbl_OrderItem>, List<OrderItemToReturnDto>>(orderFromRepo);
            foreach (var item in allItems)
            {
                item.ProductName = (await _db.ProductRepository.GetManyAsync(p => p.ID == item.ProductID, null, "")).SingleOrDefault().ProductName;
                item.ProductTotalPrice = (await _db.ProductRepository.GetManyAsync(
                    p => p.ID == item.ProductID, null, "")).SingleOrDefault().ProductPrice * Convert.ToInt32(item.ItemCount);
            }
            return Ok(allItems);
        }
    }
}