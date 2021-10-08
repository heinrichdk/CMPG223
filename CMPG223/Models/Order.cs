using System;

namespace CMPG223.Models 
{
    public class Order 
    {
        public Guid OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime DatePlaced { get; set; }
        public DateTime DateRecieved { get; set; }
    }
}
