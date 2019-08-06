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
               .Where(p => p.Id.Equals(projectId))

               .Include(p => p.Cards)
               .ThenInclude(c => c.AssigneeCards)
               .ThenInclude(ac => ac.Assignee)

               .FirstOrDefault();
        }

        public bool userCollaboratesOnProject(string assigneeName, string projectId)
        {
            var project = _myContext.Projects
              .Where(p => p.Id.Equals(projectId))

              .Include(p => p.Cards)
              .ThenInclude(c => c.AssigneeCards)
              .ThenInclude(ac => ac.Assignee)

              .FirstOrDefault();

            return project.AssigneeProjects
                .Where(ap => ap.Assignee.UserName.Equals(assigneeName))
                .Count() > 0;
        }
    }
}
