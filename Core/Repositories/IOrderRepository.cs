using API.DTOs;
using Core.Entities;

namespace Core.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByCodeAsync(long code);
        Task<int> GetQuantityOfOrdersOfAClientAsync(long code);
        Task<List<Order>> GetAllByCustomerCodeAsync(long code, PageRequest pageRequest);
    }
}
