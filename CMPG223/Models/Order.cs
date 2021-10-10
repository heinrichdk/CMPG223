using System;

namespace CMPG223.Models 
{
    public class Order 
    {
        public Guid OrderId { get; set; }
        public string OderNumber { get; set; }
        public DateTime DatePlaced { get; set; }
        public DateTime DateRecieved { get; set; }
        public Guid PlacedById { get; set; }
        public  Guid SupplierFk { get; set; }
    }
}
