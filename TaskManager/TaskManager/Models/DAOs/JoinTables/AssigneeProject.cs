using System;

namespace TaskManager.Models.DAOs.JoinTables
{
    public class AssigneeProject
    {
        public Assignee Assignee { get; set; }

        public string AssigneeId { get; set; }

        public Project Project { get; set; }

        public Guid ProjectId { get; set; }

        public AssigneeProject(Assignee assignee, Project project)
        {
            this.Assignee = assignee;
            this.AssigneeId = assignee.Id;
            this.Project = project;
            this.ProjectId = project.Id;
        }

        public AssigneeProject()
        {
        }
    }
}
