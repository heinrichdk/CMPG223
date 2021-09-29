using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMPG223.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public  string RoleName { get; set; }
        public string Description { get; set; }
    }
}