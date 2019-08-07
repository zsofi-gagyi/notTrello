using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Services
{
    public class ProjectService : IProjectService
    {
        readonly MyContext _myContext;

        public ProjectService(MyContext myContext)
        {
            _myContext = myContext;
        }

        public List<Project> GetAllFor(string userName)
        {
            return _myContext.Projects
                .Where(p => p.AssigneeProjects
                             .Any(ap => ap.Assignee.UserName.Equals(userName)))

                .Include(p => p.Cards)
                .ThenInclude(c => c.AssigneeCards)
                .ThenInclude(ac => ac.Assignee)

                .Include(p => p.AssigneeProjects)
                .ThenInclude(ap => ap.Assignee)

                .ToList();
        }

        public Project GetWithCards(string projectId)
        {
            return _myContext.Projects
               .Where(p => p.Id.ToString().Equals(projectId))

               .Include(p => p.Cards)
               .ThenInclude(c => c.AssigneeCards)
               .ThenInclude(ac => ac.Assignee)

               .FirstOrDefault();
        }

        public bool userCollaboratesOnProject(string assigneeName, string projectId)
        {
            var pereererere = _myContext.Projects.Where(p => p.Id.ToString().Equals(projectId)).FirstOrDefault();

            var project = _myContext.Projects
              .Where(p => p.Id.ToString().Equals(projectId))

              .Include(p => p.Cards)
              .ThenInclude(c => c.AssigneeCards)
              .ThenInclude(ac => ac.Assignee)

               .Include(p => p.AssigneeProjects)
               .ThenInclude(ap => ap.Assignee)

              .FirstOrDefault();

            return project.AssigneeProjects
                .Where(ap => ap.Assignee.UserName.Equals(assigneeName))
                .Count() > 0;
        }

        public void Save(Project project)
        {
            _myContext.Projects.Add(project);
            _myContext.SaveChanges();
        }

        public void Update(Project project)
        {
            _myContext.Projects.Update(project);
            _myContext.SaveChanges();
        }
    }
}
