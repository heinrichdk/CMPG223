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
        Task<List<StockDto>> GetActiveStock();
        Task<bool> UpdateStock(StockDto selectedStock);
        Task<bool> InsertStock(StockDto newStock);
        Task<List<StockDto>> GetAllStock();
        Task<List<ProjectDto>> GetProjects();
        Task<List<ProjectTypeDto>> GetProjectTypes();
        Task<bool> UpdateProject(ProjectDto selectedProject);
        Task<bool> InsertProject(ProjectDto newProject);
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

        public async Task<List<StockDto>> GetActiveStock()
        {
            var stock = await _databaseService.GetActiveStock();
            return await ConvertStockListIntoDto(stock);
        }

        public async Task<bool> UpdateStock(StockDto selectedStock)
        {
            Stock st = new Stock
            {
                StockId = selectedStock.StockId,
                MaxQty = selectedStock.MaxQty,
                SupplierFk = selectedStock.SupplierDto.SupplierId,
                CurrentQty = selectedStock.CurrentQty,
                IsActive = selectedStock.IsActive
            };
            return await _databaseService.UpdateStock(st) != 0;
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
                    SupplierFk = newStock.SupplierDto.SupplierId,
                    IsActive = newStock.IsActive
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

        public Task<List<ProjectDto>> GetProjects()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProjectTypeDto>> GetProjectTypes()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateProject(ProjectDto selectedProject)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> InsertProject(ProjectDto newProject)
        {
            throw new System.NotImplementedException();
        }

        private async Task<List<StockDto>> ConvertStockListIntoDto(List<Stock> stock)
        {
            var suppliers = await GetAllSuppliers();

            var lst = new List<StockDto>();
            foreach (var st in stock)
            {
               
                var sup = suppliers.First(x => x.SupplierId == st.SupplierFk);
                StockDto sDto = new StockDto
                {
                    StockId = st.StockId,
                    Description = st.Discription,
                    CurrentQty = st.CurrentQty,
                    MaxQty = st.MaxQty,
                    IsActive = st.IsActive,
                    SupplierDto = new SupplierDto()
                    {
                        SupplierId = sup.SupplierId,
                        Name = sup.Name,
                        IsActive = sup.IsActive,
                        Email = sup.Email,
                        ContactNumber = sup.ContactNumber
                    }
                };
                lst.Add(sDto);
            }
            return lst;
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