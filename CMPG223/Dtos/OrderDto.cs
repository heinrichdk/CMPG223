using System;
using System.Collections.Generic;


namespace CMPG223.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DatePlaced { get; set; }
        public DateTime DateRecieved { get; set; }

        public EmployeeDto PlacedBy { get; set; } = new EmployeeDto();
        public bool IsExpanded { get; set; } = false;

        public List<OrderDetailsDto> OderDetailsDto { get; set; } = new List<OrderDetailsDto>();
        public  SupplierDto Supplier { get; set; }
    }
}
