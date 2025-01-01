using API.DTOs;
using Application.Event;
using Core.Entities;

namespace Application.Services
{
    public interface IOrderService
    {
        Task<long> AddAsync(OrderCreatedEvent model);
        Task<Order> GetByCodeAsync(long code);
        Task<APIResponse<OrderResponse>> GetAllByCustomerCodeAsync(long code, PageRequest pageRequest);
    }
}
