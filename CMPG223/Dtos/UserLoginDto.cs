using System;
using CMPG223.Models;

namespace CMPG223.Dtos
{
    public class UserLoginDto
    {
        public  Guid UserLoginId { get; set; }
        public string UserName { get; set; }
        public  string Password { get; set; }
        public  bool IsActive { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}