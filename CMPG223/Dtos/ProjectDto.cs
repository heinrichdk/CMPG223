using System;

namespace CMPG223.Dtos
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public bool IsActive { get; set; }
        public ProjectTypeDto ProjectType { get; set; }
    }
}