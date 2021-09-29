using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace CMPG223.Data
{
    public class DatabaseService
    {
        public string DatabaseConnectionString = $"Data Source=DESKTOP-2CM60AH\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";

        public async Task<List<Employee>> GetEmployees()
        {
            var sql = "Select * from employees e join roles r on e.Role_Fk = r.id";
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var employees = connection.Query<Employee>(sql).ToList();

                return employees;
            }
        }
    }
}
