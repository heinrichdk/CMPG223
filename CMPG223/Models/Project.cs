using System;

namespace CMPG223.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public bool IsActive { get; set; }
        public Guid ProjectTypeFk { get; set; }
    }
}