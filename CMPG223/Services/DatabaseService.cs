using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Dtos;
using CMPG223.Models;
using Dapper;
using Microsoft.AspNetCore.Components;

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
        
    }

    public class DatabaseService : IDatabaseService
    {
        public readonly string DatabaseConnectionString =
            $"Data Source=DESKTOP-2CM60AH\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";

        public async Task<List<Employee>> GetEmployees()
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var employees = connection.Query<Employee>("SELECT * FROM employees").ToList();
                if (employees.Count == 0)
                    return new List<Employee>();

                return employees;
            }
        }
        

        public async Task<List<Employee>> GetEmployeesWhereActive()
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var employees = connection.Query<Employee>("SELECT * FROM employees where IsActive = 1").ToList();
                if (employees.Count == 0)
                    return new List<Employee>();

                return employees;
            }
        }

        public async Task<List<Role>> GetRoles()
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var roles = connection.Query<Role>("SELECT * FROM Roles").ToList();
                if (roles.Count == 0)
                    return new List<Role>();

                return roles;
            }
        }

        public async Task<List<UserLogin>> GetUserLogins()
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var userLogins = connection.Query<UserLogin>("SELECT * FROM UserLogins").ToList();
                if (userLogins.Count == 0)
                    return new List<UserLogin>();

                return userLogins;
            }
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
               return await connection.ExecuteAsync($"UPDATE Employees SET IsActive = '{employee.IsActive}', RoleFk='{employee.RoleFk}'  WHERE EmployeeId = '{employee.EmployeeId}'");
            }
        }

        public async Task<int> InsertEmployee(Employee employee)
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
               return await connection.ExecuteAsync($"INSERT INTO Employees (Name, Surname, IsActive, RoleFK)" +
                                              $" VALUES('{employee.Name}','{employee.Surname}','{employee.IsActive}','{employee.RoleFk}')");
            }
        }
    }
}