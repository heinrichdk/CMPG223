using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMPG223.Dtos
{
    public class OrdersServices
    {
        #region Property
        private readonly AppDBContext _appDBContext;
        #endregion

        #region Constructor
        public OrdersServices(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        #endregion

        #region Get List of Orders
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _appDBContext.Orders.ToListAsync();
        }
        #endregion

        #region Insert Orders
        public async Task<bool> InsertOrdersAsync(OrderDto orderDto)
        {
            await _appDBContext.Orders.AddAsync(orderDto);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Get Employee by OrderNumber
        public async Task<OrderDto> GetOrderAsync(int OderNumber)
        {
            OrderDto orderDto = await _appDBContext.Orders.FirstOrDefaultAsync(c => c.OderNumber.Equals(OderNumber));
            return orderDto;
        }
        #endregion
        #region Update Order
        public async Task<bool> UpdateOrderAsync(OrderDto orderDto)
        {
            _appDBContext.Orders.Update(orderDto);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion
        #region DeleteEmployee
        public async Task<bool> DeleteOrderAsync(OrderDto orderDto)
        {
            _appDBContext.Remove(orderDto);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}