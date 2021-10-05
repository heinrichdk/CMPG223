using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CMPG223.Dtos;
using CMPG223.Models;
using Dapper;

namespace CMPG223.Services
{
    public interface IDatabaseService
    {
        Task<List<EmployeeDto>> GetEmployees();
    }
    
    public class DatabaseService: IDatabaseService
    {
        public readonly string DatabaseConnectionString = $"Data Source=DESKTOP-2CM60AH\\SQLEXPRESS;Initial Catalog=CMPG223;Integrated Security=True";

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            await using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var employees = connection.Query<Employee>("SELECT * FROM employees").ToList();
                if (employees.Count == 0)
                    return new List<EmployeeDto>();
                    
                var roles = connection.Query<Role>("SELECT * FROM Roles").ToList();
                List<EmployeeDto> dtoList = new List<EmployeeDto>();
                foreach (var emp in employees)
                {
                    EmployeeDto empDto = new EmployeeDto
                    {
                        EmployeeId = emp.EmployeeId,
                        Name = emp.Name,
                        Surname = emp.Surname,
                        IsActive = emp.IsActive,
                        Role = roles.FirstOrDefault(x => x.RoleId == emp.RoleFk)
                    };
                    dtoList.Add(empDto);
                }
                return dtoList;
            }
        }
    }
}
