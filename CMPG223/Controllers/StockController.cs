using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Dtos;
using CMPG223.Models;
using CMPG223.Pages;
using CMPG223.Services;
using Stock = CMPG223.Models.Stock;

namespace CMPG223.Controllers
{
    public interface IStockController
    {
        Task<List<SupplierDto>> GetAllSuppliers();
        Task<bool> InsertSupplier(SupplierDto supplierDto);
        Task<bool> UpdateSupplier(SupplierDto supplierDto);
        Task<List<SupplierDto>> GetActiveSuppliers();
        Task<List<StockDto>> GetStockToOrder();
        Task<bool> UpdateStock(StockDto selectedStock);
        Task<bool> InsertStock(StockDto newStock);
        Task<List<StockDto>> GetAllStock();
        Task<List<ProjectDto>> GetProjects();
        Task<List<ProjectTypeDto>> GetProjectTypes();
        Task<bool> UpdateProject(ProjectDto selectedProject);
        Task<bool> InsertProject(ProjectDto newProject);
        Task<bool> InsertProjectType(ProjectTypeDto newType);
        Task<List<ProjectDto>> GetActiveProjects();
        Task<List<StockDto>> GetActiveStock();
        Task<bool> CheckItemsOut(List<StockCheckedOutDto> stockCheckedOutDtos);
        Task<List<StockDto>> StockBySupplier(Guid value);

        Task<List<StockCheckedOutDto>> GetCheckoutStockByStoreManager(Guid selectedStoreManagerId, DateTime startDate,
            DateTime endDate);
        Task<List<StockCheckedOutDto>> GetCheckoutStockByProjectId(Guid projectId);
    }

    public class StockController : IStockController
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

        public async Task<List<SupplierDto>> GetActiveSuppliers()
        {
            var suppliers = await _databaseService.GetActiveSuppliers();
            return ConvertSuppliersListToDto(suppliers);
        }

        public async Task<List<StockDto>> GetStockToOrder()
        {
            var stock = await _databaseService.GetStockToOrder();
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
            return !string.IsNullOrEmpty(newStock.Description) && newStock.MaxQty > 0 &&
                   newStock.CurrentQty >= 0 && newStock.SupplierDto != null && newStock.MaxQty >= newStock.CurrentQty;
        }

        public async Task<List<StockDto>> GetAllStock()
        {
            var stock = await GetStock();
            return await ConvertStockListIntoDto(stock);
        }

        public async Task<List<ProjectDto>> GetProjects()
        {
            var projects = await _databaseService.GetProjects();
            return await CreatProjectDtoList(projects);
        }

        private async Task<List<ProjectDto>> CreatProjectDtoList(List<Project> projects)
        {
            var types = await _databaseService.GetProjectTypes();
            var lst = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var type = types.First(x => x.ProjectTypeId == project.ProjectTypeFk);
                var dto = new ProjectDto()
                {
                    ProjectNumber = project.ProjectNumber,
                    ProjectId = project.ProjectId,
                    ProjectType = CreateTypeDto(type),
                    IsActive = project.IsActive
                };
                lst.Add(dto);
            }

            return lst;
        }

        public async Task<List<ProjectTypeDto>> GetProjectTypes()
        {
            List<ProjectType> types = await _databaseService.GetProjectTypes();
            return CreateTypeDtoList(types);
        }

        private List<ProjectTypeDto> CreateTypeDtoList(List<ProjectType> types)
        {
            var lst = new List<ProjectTypeDto>();
            foreach (var type in types)
            {
                var dto = CreateTypeDto(type);
                lst.Add(dto);
            }

            return lst;
        }

        private ProjectTypeDto CreateTypeDto(ProjectType type)
        {
            return new ProjectTypeDto()
            {
                Description = type.Discription,
                Name = type.Name,
                ProjectTypeId = type.ProjectTypeId
            };
        }


        public async Task<bool> UpdateProject(ProjectDto selectedProject)
        {
            return await _databaseService.UpdateProject(CreateProjectEntity(selectedProject)) != 0;
        }

        private Project CreateProjectEntity(ProjectDto dto)
        {
            return new Project()
            {
                IsActive = dto.IsActive,
                ProjectId = dto.ProjectId,
                ProjectNumber = dto.ProjectNumber,
                ProjectTypeFk = dto.ProjectType.ProjectTypeId
            };
        }


        private ProjectType CreateProjectTypeEntity(ProjectTypeDto dto)
        {
            return new ProjectType()
            {
                Discription = dto.Description,
                Name = dto.Name,
                ProjectTypeId = dto.ProjectTypeId
            };
        }


        public async Task<bool> InsertProject(ProjectDto newProject)
        {
            return await _databaseService.InsertProject(CreateProjectEntity(newProject)) != 0;
        }


        public async Task<bool> InsertProjectType(ProjectTypeDto newType)
        {
            return await _databaseService.InsertProjectType(CreateProjectTypeEntity(newType)) != 0;
        }

        public async Task<List<ProjectDto>> GetActiveProjects()
        {
            var projects = await _databaseService.GetActiveProjects();
            return await CreatProjectDtoList(projects);
        }

        public async Task<List<StockDto>> GetActiveStock()
        {
            return await ConvertStockListIntoDto(await _databaseService.GetActiveStock());
        }

        public async Task<bool> CheckItemsOut(List<StockCheckedOutDto> stockCheckedOutDtos)
        {
            foreach (var item in stockCheckedOutDtos)
            {
                var remainingStock = item.StockDto.CurrentQty - item.Qty;
                Stock st = new Stock
                {
                    StockId = item.StockDto.StockId,
                    MaxQty = item.StockDto.MaxQty,
                    SupplierFk = item.StockDto.SupplierDto.SupplierId,
                    CurrentQty = remainingStock,
                    IsActive = item.StockDto.IsActive
                };
                if (await _databaseService.UpdateStock(st) == 0)
                    return false;

                StockCheckedOut check = new StockCheckedOut()
                {
                    Date = DateTime.Now,
                    Qty = item.Qty,
                    ArtisanFk = item.Artisan.EmployeeId,
                    StoreManagerFk = item.StoreManager.EmployeeId,
                    ProjectFk = item.Project.ProjectId,
                    StockFk = item.StockDto.StockId
                };
                if (await _databaseService.StockCheckedOut(check) == 0)
                    return false;
            }

            return true;
        }

        public async Task<List<StockDto>> StockBySupplier(Guid value)
        {
            var stock = await _databaseService.GetStockBySupplier(value);
            return await ConvertStockListIntoDto(stock);
        }

        public async Task<List<StockCheckedOutDto>> GetCheckoutStockByStoreManager(Guid selectedStoreManagerId,
            DateTime startDate, DateTime endDate)
        {
            var checkedOutStock =
                await _databaseService.GetCheckoutStockByStoreManager(selectedStoreManagerId, startDate, endDate);
            return await ConvertStockCheckoutDto(checkedOutStock);
        }

        public async Task<List<StockCheckedOutDto>> GetCheckoutStockByProjectId(Guid projectId)
        {
            var checkedOutStock =
                await _databaseService.GetCheckoutStockByProjectId(projectId);
            return await ConvertStockCheckoutDto(checkedOutStock);
        }

        private async Task<List<StockCheckedOutDto>> ConvertStockCheckoutDto(List<StockCheckedOut> stockCheckedOuts)
        {
            var lst = new List<StockCheckedOutDto>();
            var employees = await _databaseService.GetEmployeesWhereActive();
            var projects = await _databaseService.GetProjects();
            var stock = await _databaseService.GetAllStock();
            foreach (var item in stockCheckedOuts)
            {
                var dt = new StockCheckedOutDto()
                {
                    Artisan = ConvertToEmployeeDto(employees.First(x => x.EmployeeId == item.ArtisanFk)),
                    Project = ConvertToProject(projects.First(x => x.ProjectId == item.ProjectFk)),
                    Qty = item.Qty,
                    StockDto = ConvertToStockDto(stock.First((x => x.StockId == item.StockFk))),
                    StoreManager = ConvertToEmployeeDto(employees.First(x => x.EmployeeId == item.StoreManagerFk)),
                    StockCheckedOutId = item.StockCheckedOutId,
                    Date = item.Date
                };
                lst.Add(dt);
            }

            return lst;
        }

        private StockDto ConvertToStockDto(Stock first)
        {
            return new StockDto()
            {
                Description = first.Discription,
                CurrentQty = first.CurrentQty,
                IsActive = first.IsActive,
                MaxQty = first.MaxQty,
                StockId = first.StockId,
                SupplierDto = new SupplierDto()
            };
        }


        private EmployeeDto ConvertToEmployeeDto(Employee emp)
        {
            return new EmployeeDto()
            {
                Name = emp.Name,
                Role = new Role(),
                Surname = emp.Surname,
                EmployeeId = emp.EmployeeId
            };
        }

        private ProjectDto ConvertToProject(Project pr)
        {
            return new ProjectDto()
            {
                ProjectId = pr.ProjectId,
                ProjectNumber = pr.ProjectNumber,
                ProjectType = new ProjectTypeDto()
            };
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
            return await _databaseService.GetAllStock();
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
            return new SupplierDto()
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