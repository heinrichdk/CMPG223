using System;

namespace CMPG223.Dtos
{
    public class SupplierDto
    {
        public  Guid SupplierId { get; set; }
        public  string Name { get; set; }
        public  string ContactNumber { get; set; }
        public  string Email { get; set; }
        public  bool IsActive { get; set; }
    }
}