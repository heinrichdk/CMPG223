using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace CMPG223.Data
{
    public interface IDatabaseService
    {
        Task<List<Employee>> GetEmployees();
    }
    
    public class DatabaseService: IDatabaseService
    {
        public readonly string DatabaseConnectionString = $"Data Source=DESKTOP-2CM60AH\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";

        public async Task<List<Employee>> GetEmployees()
        {
            const string sql = "SELECT * FROM employees e JOIN roles r ON e.RoleFk = r.RoleId";
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var employees = connection.Query<Employee>(sql).ToList();

                return employees;
            }
        }
    }
}
