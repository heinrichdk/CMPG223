using System;

namespace CMPG223.Dtos 
{
    public class OrderDetailsDto 
    {
        public Guid OrderDetailsId { get; set; }
        public StockDto StockDto { get; set; } = new StockDto();
        public int QtyOrdered { get; set; }
        public int QtyReceived { get; set; } = 0;
    }
}