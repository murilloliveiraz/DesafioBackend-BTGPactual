using API.DTOs;
using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infraestructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;
        public OrderRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Order>("orders-btg");
        }

        public async Task AddAsync(Order order)
        {
            await _collection.InsertOneAsync(order);
        }

        public async Task<List<Order>> GetAllByCustomerCodeAsync(long code, PageRequest pageRequest)
        {
            var skip = pageRequest.page * pageRequest.pageSize;
            var take = pageRequest.pageSize;

            return await _collection
                .Find(c => c.customerId == code)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public async Task<Order> GetByCodeAsync(long code)
        {
            return await _collection.Find(c => c.orderId == code).SingleOrDefaultAsync();
        }

        public async Task<int> GetQuantityOfOrdersOfAClientAsync(long code)
        {
            var count = await _collection.CountDocumentsAsync(c => c.customerId == code);
            return (int)count;
        }
    }
}
