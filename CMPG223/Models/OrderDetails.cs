using System;

namespace CMPG223.Models 
{
    public class OrderDetails 
    
   {
        public Guid OrderDetailsId { get; set; }
        public Guid StockId_fk { get; set; }
        public Guid OrderId_fk { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyReceived { get; set; }
    
    }
}
