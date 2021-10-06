using System;

namespace CMPG223.Models
{
    public class UserLogin
    {
        public Guid UserLoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public  bool IsActive { get; set; }
        public Guid EmployeeFk { get; set; }
    }
}