using System;
using CMPG223.Dtos;

namespace CMPG223.Models
{
    public class Stock
    {
        public  Guid StockId { get; set; }
        public  int MaxQty { get; set; }
        public  int CurrentQty { get; set; }
        public  string Discription { get; set; }
        public  bool IsActive { get; set; }
        public  Guid SupplierFk { get; set; }
    }
}