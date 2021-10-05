using System;

namespace CMPG223.Models
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleFk { get; set; }
    }

    public class Role
    {
        public Guid RoleId { get; set; }
        public  string RoleName { get; set; }
        public string Description { get; set; }
    }
}