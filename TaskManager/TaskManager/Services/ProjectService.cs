using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Services
{
    public class ProjectService : IProjectService
    {
        readonly MyContext _myContext;
        readonly IMapper _mapper;

        public ProjectService(MyContext myContext, IMapper mapper)
        {
            _myContext = myContext;
            _mapper = mapper;
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

        public Project Get(string projectId)
        {
            return _myContext.Projects
               .Where(p => p.Id.ToString().Equals(projectId))
               .FirstOrDefault();
        }

        public Project GetWithAssigneeProjects(string projectId)
        {
            return _myContext.Projects
               .Where(p => p.Id.ToString().Equals(projectId))
               .Include(p => p.AssigneeProjects)
               .FirstOrDefault();
        }

        public bool UserIsCollaboratingOnProject(string assigneeName, string projectId)
        {
            var project = _myContext.Projects
               .Where(p => p.Id.ToString().Equals(projectId))

                .Include(p => p.AssigneeProjects)
                    .ThenInclude(ap => ap.Assignee)

              .FirstOrDefault();

            return project.AssigneeProjects.Where(ap => ap.Assignee.UserName.Equals(assigneeName)).Count() > 0;
        }

        public void Save(Project project)
        {
            _myContext.Projects.Add(project);
            _myContext.SaveChanges();
        }

        public void Update(Project project)
        {
            var originalProject = _myContext.FindAsync(typeof (Project), project.Id).Result;
            _myContext.Entry(originalProject).State = EntityState.Detached;
            _myContext.Entry(project).State = EntityState.Modified;
            _myContext.SaveChanges();
        }

        public void TranslateAndUpdate(ProjectWithCardsDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);

            var cards = new List<Card>();
            foreach (var cardDTO in projectDTO.CardWithAssigneesDTOs)
            {
                var assigneeCards = new List<AssigneeCard>();
                var newCard = _mapper.Map<Card>(cardDTO);
                newCard.AssigneeCards = new List<AssigneeCard>();

                foreach (var assigneeDTO in cardDTO.AssigneeDTOs)
                {
                    var assignee = _mapper.Map<Assignee>(assigneeDTO);
                    var newAssigneeCard = new AssigneeCard(assignee, newCard);

                    newCard.AssigneeCards.Add(newAssigneeCard);
                }

                cards.Add(newCard);
            }
            project.Cards = cards;

            var assigneeProjects = new List<AssigneeProject>();
            foreach (var assigneeDTO in projectDTO.AssigneeDTOs)
            {
                assigneeProjects.Add(new AssigneeProject (_mapper.Map<Assignee>(assigneeDTO), project));
            }
            project.AssigneeProjects = assigneeProjects;

            Update(project);
        }

        public void Delete(string projectId)
        {
            _myContext.AssigneeProjects.RemoveRange(_myContext.AssigneeProjects.Where(ap => ap.ProjectId.ToString().Equals(projectId)));
            _myContext.Cards.RemoveRange(_myContext.Cards.Where(ap => ap.Project.Id.ToString().Equals(projectId)));
            _myContext.Projects.Remove(_myContext.Projects.Where(p => p.Id.ToString().Equals(projectId)).First());
            _myContext.SaveChanges();
        }
    }
}