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
        Task<bool> UpdateOrder(OrderDto orderDto);
        Task<bool> InsertOrder(OrderDto orderDto);
        

    public class OrderController 
    {
        public Task<bool> UpdateOrder(OrderDto selectedOrder)
        {
            throw new System.NotImplementedException();
        }
       

       

    }
}