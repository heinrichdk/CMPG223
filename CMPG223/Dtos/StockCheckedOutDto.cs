using System;

namespace CMPG223.Dtos
{
    public class StockCheckedOutDto
    {
        public  Guid StockCheckedOutId { get; set; }
        public StockDto StockDto { get; set; } = new StockDto();
        public EmployeeDto Artisan { get; set; } = new EmployeeDto();
        public EmployeeDto StoreManager { get; set; } = new EmployeeDto();
        public ProjectDto Project { get; set; } = new ProjectDto();
        public  int Qty { get; set; }
        public  DateTime Date { get; set; }
    }
}