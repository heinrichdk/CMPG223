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
        Task<bool> InsertOrder(OrderDto orderDto);
        Task<List<OrderDto>> GetPendingOrders();
        Task<bool> ReceiveOrder(OrderDto newOrder);
    }

    public class OrderController : ISOrderController
    {
        private readonly IDatabaseService _databaseService;

        public OrderController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        

        public async Task<bool> InsertOrder(OrderDto orderDto)
        {
            bool success = true;
            if (CheckOrdersDto(orderDto))
            {
                Order order = new Order
                {
                    OderNumber = orderDto.OrderNumber,
                    DatePlaced = DateTime.Now,
                    PlacedById = orderDto.PlacedBy.EmployeeId,
                    SupplierFk = orderDto.Supplier.SupplierId
                };
                var id = await _databaseService.InsertOrder(order);
                if (id == Guid.Empty)
                    return false;
                else
                    foreach (var detail in orderDto.OderDetailsDto)
                    {
                        OrderDetails od = new OrderDetails()
                        {
                            OrderFk = id,
                            QtyOrdered = detail.QtyOrdered,
                            StockFk = detail.StockDto.StockId
                        };
                        success = await _databaseService.InsertOderDetails(od) != 0;
                        if (!success)
                            return false;
                    }
            }

            return true;
        }

        public async Task<bool> UpdateOrder(OrderDto orderDto)
        {
            Order order = new Order
            {
                OderNumber = orderDto.OrderNumber,
                DatePlaced = orderDto.DatePlaced,
                DateRecieved = orderDto.DateRecieved,
                //PlacedById = orderDto.PlacedById
            };
            return await _databaseService.UpdateOrder(order) != 0;
        }

        public async Task<List<OrderDto>> GetPendingOrders()
        {
            var orders = await _databaseService.GetPendingOrders();
            List<OrderDto> lst = new List<OrderDto>();
            var suppliers = await _databaseService.GetSuppliers();
            var employees = await _databaseService.GetEmployees();
            var orderDetails = await _databaseService.GetOrderDetails();
            var stock = await _databaseService.GetAllStock();

            foreach (var order in orders)
            {
                OrderDto dto = new OrderDto()
                {
                    Supplier = CreateSuppierDto(suppliers.First(x => x.SupplierId == order.SupplierFk)),
                    DatePlaced = order.DatePlaced,
                    OrderId = order.OrderId,
                    OrderNumber = order.OderNumber,
                    PlacedBy = CreateEmployeeDto(employees.First(x => x.EmployeeId == order.PlacedById)),
                    OderDetailsDto = new List<OrderDetailsDto>()
                };
                foreach (var od in orderDetails.FindAll(x => x.OrderFk == order.OrderId))
                {
                    dto.OderDetailsDto.Add(CreateDetailsDto(od, stock.First(x => x.StockId == od.StockFk)));
                }

                lst.Add(dto);
            }

            return lst;
        }

        public async Task<bool> ReceiveOrder(OrderDto newOrder)
        {
            Order dto = new Order()
            {
                DateRecieved = DateTime.Now,
                OrderId = newOrder.OrderId
            };
            foreach (var od in newOrder.OderDetailsDto)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    QtyRecieved = od.QtyReceived,
                    OrderDetailsId = od.OrderDetailsId
                };
                if ( await _databaseService.ReceiveOrderDetails(orderDetails) == 0)
                    return false;
                Stock st = new Stock()
                {
                    CurrentQty = od.StockDto.CurrentQty + od.QtyReceived,
                    StockId = od.StockDto.StockId
                };
                if ( await _databaseService.UpdateStockQty(st) == 0)
                    return false;
            }
            if ( await _databaseService.ReceiveOrder(dto) == 0)
                return false;
            return true;
        }

        private OrderDetailsDto CreateDetailsDto(OrderDetails od, Stock stock)
        {
            return new OrderDetailsDto
            {
                QtyOrdered = od.QtyOrdered,
                QtyReceived = od.QtyRecieved,
                OrderDetailsId = od.OrderDetailsId,
                StockDto = new StockDto()
                {
                    Description = stock.Discription,
                    CurrentQty = stock.CurrentQty,
                    IsActive = stock.IsActive,
                    MaxQty = stock.MaxQty,
                    StockId = stock.StockId,
                    SupplierDto = new SupplierDto()
                }
            };
        }

        private EmployeeDto CreateEmployeeDto(Employee first)
        {
            return new EmployeeDto()
            {
                EmployeeId = first.EmployeeId,
                Name = first.Name,
                Surname = first.Surname
            };
        }

        private SupplierDto CreateSuppierDto(Supplier first)
        {
            return new SupplierDto()
            {
                Email = first.Email,
                Name = first.Name, ContactNumber = first.ContactNumber,
                IsActive = first.IsActive,
                SupplierId = first.SupplierId
            };
        }

        private bool CheckOrdersDto(OrderDto newOrder)
        {
            return !string.IsNullOrWhiteSpace(newOrder.OrderNumber);
        }

        private async Task<List<Order>> GetOrders()
        {
            return await _databaseService.GetOrders();
        }
    }
}