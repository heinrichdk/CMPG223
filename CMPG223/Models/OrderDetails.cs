using System;

namespace CMPG223.Models 
{
    public class OrderDetails 
    
   {
        public Guid OrderDetailsId { get; set; }
        public Guid StockFk { get; set; }
        public Guid OrderFk { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyRecieved { get; set; }
    
    }
}
