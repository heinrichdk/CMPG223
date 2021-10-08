using System;


namespace CMPG223.Dtos 
{
    public class OrderDto 
    {
        public Guid Placedbyid { get; set; }
        public int OrderNumber { get; set; }
        public DateTime DatePlaced { get; set; }
        public DateTime DateRecieved { get; set; }
    }
}