using API.DTOs;
using Application.Event;
using Core.Entities;
using Core.Repositories;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<long> AddAsync(OrderCreatedEvent model)
        {
            var order = model.ToEntity();

            await _orderRepository.AddAsync(order);

            return order.orderId;
        }

        public async Task<APIResponse<OrderResponse>> GetAllByCustomerCodeAsync(long code, PageRequest pageRequest)
        {
            var orders = await _orderRepository.GetAllByCustomerCodeAsync(code, pageRequest);
            var totalOrders = await _orderRepository.GetQuantityOfOrdersOfAClientAsync(code);

            var orderResponses = orders.Select(order => new OrderResponse().ToOrderResponse(order)).ToList();

            var paginationResponse = new PaginationResponse(
                page: pageRequest.page,
                pageSize: pageRequest.pageSize,
                totalElements: totalOrders,
                totalPages: (int)Math.Ceiling(totalOrders / (double)pageRequest.pageSize)
            );

            var response = new APIResponse<OrderResponse>(orderResponses, paginationResponse);

            return response;
        }

        public async Task<Order> GetByCodeAsync(long code)
        {
            var order = await _orderRepository.GetByCodeAsync(code);

            return order;
        }
    }
}
