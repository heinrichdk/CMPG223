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
        Task<List<UserLogin>> GetUserLogins();
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
        Task<int> InsertOrder(Order order);
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
    }

    public class DatabaseService : IDatabaseService
    {
        //Morena String
        // private readonly string _databaseConnectionString =
        // $"Data Source=DESKTOP-2CM60AH\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";
        private readonly string _databaseConnectionString =
            $"Data Source=TINUSLAPTOP;Initial Catalog=CMPG223;Integrated Security=True";

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

        public async Task<List<UserLogin>> GetUserLogins()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var userLogins = connection.Query<UserLogin>("SELECT * FROM UserLogins").ToList();
            return userLogins.Count == 0 ? new List<UserLogin>() : userLogins;
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

        public async Task<int> InsertOrder(Order order)
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            return await connection.ExecuteAsync(
                $"INSERT INTO Orders (OderNumber, DatePlaced, DateRecieved, PlacedById)" +
                $" VALUES('{order.OderNumber}','{order.DatePlaced}','{order.DateRecieved}','{order.PlacedById}')");
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
    }
}