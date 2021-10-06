using System;

namespace CMPG223.Dtos
{
    public class StockDto
    {
        public  Guid StockId { get; set; }
        public  int MaxQty { get; set; }
        public  int CurrentQty { get; set; }
        public  string Description { get; set; }
        public  SupplierDto SupplierDto { get; set; }
    }
}