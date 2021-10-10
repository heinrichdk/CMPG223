using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Models;
using CMPG223.Pages;
using Dapper;
using Stock = CMPG223.Models.Stock;

namespace CMPG223.Services
{
    public interface IDatabaseService
    {
        Task<List<Order>> GetOrders();
        Task<List<Employee>> GetEmployees();
        Task<List<Employee>> GetEmployeesWhereActive();
        Task<List<Role>> GetRoles();
        Task<int> UpdateEmployee(Employee employee);
        Task<int> InsertEmployee(Employee employee);
        Task<List<Supplier>> GetSuppliers();
        Task<int> InsertSupplier(Supplier supplier);
        Task<int> UpdateSupplier(Supplier supplier);
        Task<List<Supplier>> GetActiveSuppliers();
        Task<List<Stock>> GetStockToOrder();
        Task<List<Stock>> GetAllStock();
        Task<int> InsertStock(Stock newStock);
        Task<int> UpdateStock(Stock selectedStock);
        Task<Guid> InsertOrder(Order order);
        Task<int> UpdateOrder(Order order);
        Task<List<Project>> GetProjects();
        Task<List<ProjectType>> GetProjectTypes();
        Task<int> UpdateProject(Project createProjectEntity);
        Task<int> InsertProject(Project createProjectEntity);
        Task<int> InsertProjectType(ProjectType createProjectTypeEntity);
        Task<List<Employee>> GetActiveEmployeesByRole(string role);
        Task<List<Project>> GetActiveProjects();
        Task<List<Stock>> GetActiveStock();
        Task<int> StockCheckedOut(StockCheckedOut check);
        Task<List<Stock>> GetStockBySupplier(Guid value);
        Task<int> InsertOderDetails(OrderDetails od);
        Task<List<Order>> GetPendingOrders();
        Task<List<OrderDetails>> GetOrderDetails();
        Task<int> ReceiveOrderDetails(OrderDetails orderDetails);
        Task<int> ReceiveOrder(Order order);
        Task<int> UpdateStockQty(Stock st);
        Task<List<StockCheckedOut>> GetCheckoutStockByStoreManager(Guid selectedStoreManagerId, DateTime startDate, DateTime endDate);
        Task<List<StockCheckedOut>> GetCheckoutStockByProjectId(Guid projectId);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string _databaseConnectionString =
           // $"Data Source=PCD-MOREMOEK\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";
           // $"Data Source=TINUSLAPTOP;Initial Catalog=CMPG223;Integrated Security=True";
           $"Data Source=TINUSLAPTOP;Initial Catalog=CMPG223;Integrated Security=True";

        private IDatabaseService _databaseServiceImplementation;

        public async Task<List<Employee>> GetEmployees()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var employees = connection.Query<Employee>("SELECT * FROM employees").ToList();
            return employees.Count == 0 ? new List<Employee>() : employees;
        }

        public async Task<List<Order>> GetOrders()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var orders = connection.Query<Order>("SELECT * FROM orders").ToList();
            return orders.Count == 0 ? new List<Order>() : orders;
        }


        public async Task<List<Employee>> GetEmployeesWhereActive()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var employees = connection.Query<Employee>("SELECT * FROM employees where IsActive = 1").ToList();
            return employees.Count == 0 ? new List<Employee>() : employees;
        }

        public async Task<List<Role>> GetRoles()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var roles = connection.Query<Role>("SELECT * FROM Roles").ToList();
            return roles.Count == 0 ? new List<Role>() : roles;
        }
        

        public async Task<int> UpdateEmployee(Employee employee)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Employees SET IsActive = '{employee.IsActive}', RoleFk='{employee.RoleFk}'  WHERE EmployeeId = '{employee.EmployeeId}'");
        }

        public async Task<int> InsertEmployee(Employee employee)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync($"INSERT INTO Employees (Name, Surname, IsActive, RoleFK)" +
                                                 $" VALUES('{employee.Name}','{employee.Surname}','{employee.IsActive}','{employee.RoleFk}')");
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var suppliers = connection.Query<Supplier>("SELECT * FROM Suppliers").ToList();
            return suppliers.Count == 0 ? new List<Supplier>() : suppliers;
        }

        public async Task<int> InsertSupplier(Supplier supplier)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync($"INSERT INTO Suppliers (Name, Email, ContactNumber, IsActive)" +
                                                 $" VALUES('{supplier.Name}','{supplier.Email}','{supplier.ContactNumber}','{supplier.IsActive}')");
        }

        public async Task<Guid> InsertOrder(Order order)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var sql =
                $"DECLARE @RSGUID uniqueidentifier; SET @RSGUID = NEWID();INSERT INTO Orders (OrderId, OderNumber, DatePlaced, PlacedById, SupplierFk)" +
                $" VALUES(@RSGUID,'{order.OderNumber}','{order.DatePlaced}','{order.PlacedById}', '{order.SupplierFk}')" +
                $" Select @RSGUID";
            return await connection.ExecuteScalarAsync<Guid>(sql);
        }

        public async Task<int> UpdateOrder(Order order)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Orders SET   OderNumber='{order.OderNumber}', DatePlaced='{order.DatePlaced}', DateRecieved='{order.DateRecieved}'  WHERE OrderId = '{order.OrderId}'");
        }

        public async Task<int> UpdateSupplier(Supplier supplier)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Suppliers SET IsActive = '{supplier.IsActive}', Name='{supplier.Name}', Email='{supplier.Email}', ContactNumber='{supplier.ContactNumber}'  WHERE SupplierId = '{supplier.SupplierId}'");
        }

        public async Task<List<Supplier>> GetActiveSuppliers()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var suppliers = connection.Query<Supplier>("SELECT * FROM Suppliers WHERE IsActive = '1'").ToList();
            return suppliers.Count == 0 ? new List<Supplier>() : suppliers;
        }

        public async Task<List<Stock>> GetStockToOrder()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var stock = connection.Query<Stock>("SELECT st.* FROM Stock st JOIN Suppliers sup ON st.SupplierFk = sup.SupplierId WHERE sup.IsActive = '1' AND st.IsActive = 'true' AND st.MaxQty * 0.3 >= st.CurrentQty").ToList();
            return stock.Count == 0 ? new List<Stock>() : stock;
        }

        public async Task<List<Stock>> GetAllStock()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var stock = connection.Query<Stock>("SELECT * FROM Stock").ToList();
            return stock.Count == 0 ? new List<Stock>() : stock;
        }

        public async Task<int> InsertStock(Stock newStock)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO Stock (Discription, SupplierFk, MaxQty, CurrentQty, IsActive)" +
                $" VALUES('{newStock.Discription}','{newStock.SupplierFk}','{newStock.MaxQty}','{newStock.CurrentQty}', '{newStock.IsActive}')");
        }

        public async Task<int> UpdateStock(Stock selectedStock)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Stock SET  SupplierFk = '{selectedStock.SupplierFk}', MaxQty = '{selectedStock.MaxQty}', CurrentQty ='{selectedStock.CurrentQty}', IsActive = '{selectedStock.IsActive}' WHERE StockId = '{selectedStock.StockId}'");
        }

        public async Task<List<Project>> GetProjects()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var projects = connection.Query<Project>("SELECT * FROM Projects").ToList();
            return projects.Count == 0 ? new List<Project>() : projects;
        }

        public async Task<List<ProjectType>> GetProjectTypes()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var type = connection.Query<ProjectType>("SELECT * FROM ProjectType").ToList();
            return type.Count == 0 ? new List<ProjectType>() : type;
        }

        public async Task<int> UpdateProject(Project createProjectEntity)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Projects SET  IsActive = '{createProjectEntity.IsActive}' WHERE ProjectId = '{createProjectEntity.ProjectId}'");
        }

        public async Task<int> InsertProject(Project createProjectEntity)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO Projects (IsActive , ProjectNumber, ProjectTypeFk)" +
                $" VALUES('{createProjectEntity.IsActive}','{createProjectEntity.ProjectNumber}','{createProjectEntity.ProjectTypeFk}')");
        }


        public async Task<int> InsertProjectType(ProjectType createProjectTypeEntity)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO ProjectType (Discription, Name)" +
                $" VALUES('{createProjectTypeEntity.Discription}','{createProjectTypeEntity.Name}')");
        }

        public async Task<List<Employee>> GetActiveEmployeesByRole(string role)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var sql =
                $"SELECT e.* FROM Employees e JOIN Roles r ON r.RoleId = e.RoleFk WHERE r.RoleName = '{role}' AND e.IsActive = '1'";
            var employees = connection
                .Query<Employee>(sql)
                .ToList();
            return employees.Count == 0 ? new List<Employee>() : employees;
        }

        public async Task<List<Project>> GetActiveProjects()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var projects = connection.Query<Project>("SELECT * FROM Projects WHERE IsActive = '1'").ToList();
            return projects.Count == 0 ? new List<Project>() : projects;
        }

        public async Task<List<Stock>> GetActiveStock()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var stock = connection.Query<Stock>("SELECT * FROM Stock WHERE IsActive = '1'").ToList();
            return stock.Count == 0 ? new List<Stock>() : stock;
        }

        public async Task<int> StockCheckedOut(StockCheckedOut check)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO StockCheckedOut (Qty , Date, StoreManagerFk, ArtisanFk, ProjectFk, StockFk)" +
                $" VALUES('{check.Qty}','{check.Date}','{check.StoreManagerFk}','{check.ArtisanFk}','{check.ProjectFk}','{check.StockFk}')");
        }

        public async Task<List<Stock>> GetStockBySupplier(Guid value)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var stock = connection.Query<Stock>($"SELECT * FROM Stock WHERE IsActive = '1' AND supplierFk = '{value}'")
                .ToList();
            return stock.Count == 0 ? new List<Stock>() : stock;
        }

        public async Task<int> InsertOderDetails(OrderDetails od)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO OrderDetails (OrderFk , StockFk, QtyRecieved, QtyOrdered)" +
                $" VALUES('{od.OrderFk}','{od.StockFk}', 0 ,'{od.QtyOrdered}')");
        }

        public async Task<List<Order>> GetPendingOrders()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var orders = connection.Query<Order>($"SELECT * FROM Orders WHERE DateRecieved IS NULL")
                .ToList();
            return orders.Count == 0 ? new List<Order>() : orders;
        }

        public async Task<List<OrderDetails>> GetOrderDetails()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var orders = connection
                .Query<OrderDetails>(
                    $"SELECT * FROM OrderDetails od JOIN orders o on O.OrderId = od.OrderFk WHERE o.DateRecieved IS NULL")
                .ToList();
            return orders.Count == 0 ? new List<OrderDetails>() : orders;
        }

        public async Task<int> ReceiveOrderDetails(OrderDetails orderDetails)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE OrderDetails SET QtyRecieved = {orderDetails.QtyRecieved} WHERE OrderDetailsId = '{orderDetails.OrderDetailsId}'");
        }

        public async Task<int> ReceiveOrder(Order order)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Orders SET DateRecieved = '{order.DateRecieved}' WHERE OrderId = '{order.OrderId}'");
        }

        public async Task<int> UpdateStockQty(Stock st)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"UPDATE Stock SET  CurrentQty = '{st.CurrentQty}' WHERE StockId = '{st.StockId}'");
        }

        public async Task<List<StockCheckedOut>> GetCheckoutStockByStoreManager(Guid selectedStoreManagerId, DateTime startDate, DateTime endDate)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var st = connection
                .Query<StockCheckedOut>(
                    $"SELECT * FROM StockCheckedOut  WHERE StoreManagerFk = '{selectedStoreManagerId}' AND  CAST(Date AS DATE) <=  CAST('{endDate.Date}' AS DATE) AND CAST(Date AS DATE) >= CAST('{startDate.Date}' AS DATE)")
                .ToList();
            return st.Count == 0 ? new List<StockCheckedOut>() : st;
        }
        public async Task<List<StockCheckedOut>> GetCheckoutStockByProjectId(Guid projectId)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var st = connection
                .Query<StockCheckedOut>(
                    $"SELECT * FROM StockCheckedOut  WHERE ProjectFk = '{projectId}'")
                .ToList();
            return st.Count == 0 ? new List<StockCheckedOut>() : st;
        }
    }
}