using System;
using System.Data;

namespace CMPG223.Models
{
    public class StockCheckedOut
    {
        public  Guid StockCheckedOutId { get; set; }
        public Guid StockFk { get; set; }
        public Guid ArtisanFk { get; set; } 
        public Guid StoreManagerFk { get; set; } 
        public Guid ProjectFk { get; set; } 
        public  int Qty { get; set; }
        public  DateTime Date { get; set; }
    }
}