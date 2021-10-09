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
        Task<List<Stock>> GetActiveStock();
        Task<List<Stock>> GetAllStock();
        Task<int> InsertStock(Stock newStock);
        Task<int> UpdateStock(Stock selectedStock);
        Task<List<Project>> GetProjects();
        Task<List<ProjectType>> GetProjectTypes();
        Task<int> UpdateProject(Project createProjectEntity);
        Task<int> InsertProject(Project createProjectEntity);
        Task<int> InsertProjectType(ProjectType createProjectTypeEntity);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string _databaseConnectionString =
            $"Data Source=TINUSLAPTOP;Initial Catalog=CMPG223;Integrated Security=True";

        public async Task<List<Employee>> GetEmployees()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var employees = connection.Query<Employee>("SELECT * FROM employees").ToList();
            return employees.Count == 0 ? new List<Employee>() : employees;
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

        public async Task<List<Stock>> GetActiveStock()
        {
            await using var connection = new SqlConnection(_databaseConnectionString);
            var stock = connection.Query<Stock>("SELECT * FROM Stock WHERE IsActive = '1'").ToList();
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
    }
}