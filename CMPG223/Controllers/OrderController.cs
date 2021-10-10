using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Dtos;
using CMPG223.Models;
using CMPG223.Services;

namespace CMPG223.Controllers
{
    public interface ISOrderController
    {
        Task<List<OrderDto>> GetAllOrders();
        Task<bool> InsertOrder(OrderDto orderDto);
        Task<bool> UpdateOrder(OrderDto orderDto);
        


      
    }

    public class OrderController:ISOrderController 
    {
        private readonly IDatabaseService _databaseService;
        public OrderController(IDatabaseService databaseService) 
        {
            _databaseService = databaseService;
        }

        public async Task <List<OrderDto>> GetAllOrders() 
        {
            var orders = await _databaseService.GetOrders();
            return await ConvertOrdersListDto(orders).ConfigureAwait(false);
        }

        public async Task<bool> InsertOrder(OrderDto orderDto) 
        {
            if (CheckOrdersDto(orderDto)) 
            {
                Order order = new Order
                {
                    OderNumber = orderDto.OderNumber,
                    DatePlaced = orderDto.DatePlaced,
                    DateRecieved = orderDto.DateRecieved,
                    PlacedById = orderDto.PlacedById
                };
                return await _databaseService.InsertOrder(order) != 0;
            }
            return false;
        }
        public async Task<bool> UpdateOrder(OrderDto orderDto) 
        {
            Order order = new Order
            {
                OderNumber = orderDto.OderNumber,
                DatePlaced = orderDto.DatePlaced,
                DateRecieved = orderDto.DateRecieved,
                PlacedById = orderDto.PlacedById
            };
            return await _databaseService.UpdateOrder(order) != 0;
        }
        private bool CheckOrdersDto(OrderDto newOrder)
        {
           return !string.IsNullOrWhiteSpace(newOrder.OderNumber);
         }
        private async Task<List<Order>> GetOrders()
        {
            return await _databaseService.GetOrders();
        }

        private async Task< List<OrderDto> >ConvertOrdersListDto(List<Order> order) 
        {
            var placedorders = await GetAllOrders();

            var lst = new List<OrderDto>();
            foreach (var st in order)
            {

                
                OrderDto sDto = new OrderDto
                {
                    OrderId = st.OrderId,
                    OderNumber = st.OderNumber,
                    DatePlaced = st.DatePlaced,
                    DateRecieved = st.DateRecieved,
                    PlacedById = st.PlacedById,
                  //  SupplierDto = new SupplierDto()
                   //// {
                    //    SupplierId = sup.SupplierId,
                    //    Name = sup.Name,
                    //    IsActive = sup.IsActive,
                  //     Email = sup.Email,
                  //      ContactNumber = sup.ContactNumber
                 //   }
                };
                lst.Add(sDto);
            }
            return lst;
        }

        private OrderDto ConvertOrderToDto(Order order)
        {
            return new OrderDto()
            {
                PlacedById = order.PlacedById,
                DateRecieved = order.DateRecieved,
                DatePlaced = order.DatePlaced,
                OderNumber = order.OderNumber,
                OrderId = order.OrderId
            };
        }
    }
}