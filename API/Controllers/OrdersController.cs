using API.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("customers/{customerId}/orders/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllOrders(long customerId, int page = 0, int pageSize = 10)
        {
            var pageRequest = new PageRequest(page, pageSize);
            var ordersResponse = await _orderService.GetAllByCustomerCodeAsync(customerId, pageRequest);

            if (ordersResponse.data == null || !ordersResponse.data.Any())
            {
                return NotFound("This client doesn't have any order");
            }

            return Ok(ordersResponse);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrder(long orderId)
        {
            var order = await _orderService.GetByCodeAsync(orderId);

            if (order == null)
            {
                return BadRequest("This order doesn't exist");
            }

            return Ok(order);
        }
    }
}
