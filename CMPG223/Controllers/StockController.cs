using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Dtos;
using CMPG223.Models;
using CMPG223.Services;

namespace CMPG223.Controllers
{

    public interface IStockController
    {
        Task<List<SupplierDto>> GetAllSuppliers();
        Task<bool> InsertSupplier(SupplierDto supplierDto);
        Task<bool> UpdateSupplier(SupplierDto supplierDto);
        Task<List<SupplierDto>> GetActiveSuppliers();
        Task<bool> UpdateStock(StockDto selectedStock);
        Task<bool> InsertStock(StockDto newStock);
        Task<List<StockDto>> GetAllStock();
    }
    
    public class StockController:IStockController
    {
        private readonly IDatabaseService _databaseService;

        public StockController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<SupplierDto>> GetAllSuppliers()
        {
            var suppliers = await _databaseService.GetSuppliers();
            return ConvertSuppliersListToDto(suppliers);
        }

        public async Task<bool> InsertSupplier(SupplierDto supplierDto)
        {
            if (CheckSupplierDto(supplierDto))
            {
                Supplier supplier = new Supplier
                {
                    Name = supplierDto.Name,
                    Email = supplierDto.Email,
                    IsActive = supplierDto.IsActive,
                    ContactNumber = supplierDto.ContactNumber
                };
                return await _databaseService.InsertSupplier(supplier) != 0;
            }

            return false;
        }

        public async Task<bool> UpdateSupplier(SupplierDto supplierDto)
        {
            if (CheckSupplierDto(supplierDto))
            {
                Supplier supplier = new Supplier
                {
                    SupplierId = supplierDto.SupplierId,
                    Name = supplierDto.Name,
                    Email = supplierDto.Email,
                    IsActive = supplierDto.IsActive,
                    ContactNumber = supplierDto.ContactNumber
                };
                return await _databaseService.UpdateSupplier(supplier) != 0;
            }

            return false;
        }

        public async  Task<List<SupplierDto>> GetActiveSuppliers()
        {
            var suppliers = await _databaseService.GetActiveSuppliers();
            return ConvertSuppliersListToDto(suppliers);
        }

        public Task<bool> UpdateStock(StockDto selectedStock)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> InsertStock(StockDto newStock)
        {
            if (CheckStockDto(newStock))
            {
                Stock stock = new Stock
                {
                    Discription = newStock.Description,
                    CurrentQty = newStock.CurrentQty,
                    MaxQty = newStock.MaxQty,
                    SupplierFk = newStock.SupplierDto.SupplierId
                };
                return await _databaseService.InsertStock(stock) != 0;
            }

            return false;
        }

        private bool CheckStockDto(StockDto newStock)
        {
            return !string.IsNullOrEmpty(newStock.Description) && newStock.MaxQty > 0  &&
                   newStock.CurrentQty >= 0 &&  newStock.SupplierDto != null && newStock.MaxQty >= newStock.CurrentQty;
        }

        public async Task<List<StockDto>> GetAllStock()
        {
            var stock = await GetStock();
            return await ConvertStockListIntoDto(stock);
        }

        private async Task<List<StockDto>> ConvertStockListIntoDto(List<Stock> stock)
        {
            var suppliers = await GetAllSuppliers();

            return stock.Select(emp => new StockDto
                {
                    StockId = emp.StockId,
                    Description = emp.Discription,
                    MaxQty = emp.MaxQty,
                    CurrentQty = emp.CurrentQty,
                    SupplierDto = suppliers.FirstOrDefault(x => x.SupplierId == emp.SupplierFk)
                })
                .ToList();
        }

        private async Task<List<Stock>> GetStock()
        {
            return  await _databaseService.GetAllStock();
        }

        private bool CheckSupplierDto(SupplierDto supplierDto)
        {
            return !string.IsNullOrEmpty(supplierDto.Name) && !string.IsNullOrEmpty(supplierDto.ContactNumber) &&
                   !string.IsNullOrEmpty(supplierDto.Email);
        }

        private List<SupplierDto> ConvertSuppliersListToDto(List<Supplier> suppliers)
        {
            var supplierDtos = new List<SupplierDto>();
            foreach (var supplier in suppliers)
            {
                supplierDtos.Add(ConvertSupplierToDto(supplier));
            }

            return supplierDtos;
        }

        private SupplierDto ConvertSupplierToDto(Supplier supplier)
        {
            return  new SupplierDto()
            {
                Email = supplier.Email,
                Name = supplier.Name,
                ContactNumber = supplier.ContactNumber,
                IsActive = supplier.IsActive,
                SupplierId = supplier.SupplierId
            };
        }
        
    }
}