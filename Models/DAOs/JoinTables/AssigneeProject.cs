using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DAOs.JoinTables
{
    public class AssigneeProject
    {
        public Assignee Assignee { get; set; }

        public string AssigneeId { get; set; }

        public Project Project { get; set; }

        public Guid ProjectId { get; set; }

        public AssigneeProject(Assignee assignee, Project project)
        {
            Assignee = assignee;
            AssigneeId = assignee.Id;
            Project = project;
            ProjectId = project.Id;
        }

        public AssigneeProject()
        {
        }
    }
}
