using System;
using CMPG223.Models;

namespace CMPG223.Dtos
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public Role Role { get; set; } = new Role();
    }
}