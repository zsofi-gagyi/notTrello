using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Models.DAOs.JoinTables;
using TaskManager.Models.DTOs;
using TaskManager.Repository;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services
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

        public Project GetWithCards(Guid projectId)
        {
            return _myContext.Projects
                .Where(p => p.Id.Equals(projectId))

                .Include(p => p.Cards)
                    .ThenInclude(c => c.AssigneeCards)
                        .ThenInclude(ac => ac.Assignee)

                .Include(p => p.AssigneeProjects)
                    .ThenInclude(ap => ap.Assignee)

               .FirstOrDefault();
        }

        public Project Get(Guid projectId)
        {
            return _myContext.Projects
               .FirstOrDefault(p => p.Id
                                     .Equals(projectId));
        }

        public Project GetWithAssigneeProjects(Guid projectId)
        {
            return _myContext.Projects
               .Where(p => p.Id
                            .Equals(projectId))
               .Include(p => p.AssigneeProjects)
               .FirstOrDefault();
        }

        public bool UserIsCollaboratingOnProject(string assigneeName, Guid projectId)
        {
            var project = _myContext.Projects
               .Where(p => p.Id.Equals(projectId))

                .Include(p => p.AssigneeProjects)
                    .ThenInclude(ap => ap.Assignee)

              .FirstOrDefault();

            return project.AssigneeProjects
                .Where(ap => ap
                            .Assignee
                            .UserName
                            .Equals(assigneeName))
                .Count() > 0;
        }

        public void Save(Project project)
        {
            _myContext.Projects.Add(project);
            _myContext.SaveChanges();
        }

        public void Update(Project project)
        {
            var originalProject = _myContext.Find(typeof (Project), project.Id); 
            _myContext.Entry(originalProject).State = EntityState.Detached;
            _myContext.Entry(project).State = EntityState.Modified;
            _myContext.SaveChanges();
        }

        public Project TranslateToProject(ProjectWithCardsDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);

            project.Cards = extractCardsFrom(projectDTO);
            project.AssigneeProjects = extractAssigneeProjectsFrom(projectDTO, project);

            return project;
        }

        private List<Card> extractCardsFrom(ProjectWithCardsDTO projectDTO)
        {
            var cards = new List<Card>();
            foreach (var cardDTO in projectDTO.CardWithAssigneesDTOs)
            {
                var newCard = _mapper.Map<Card>(cardDTO);

                newCard.AssigneeCards = extractAssigneeCardsFrom(cardDTO.AssigneeDTOs, newCard);
                cards.Add(newCard);
            }
            return cards;
        }

        private List<AssigneeCard> extractAssigneeCardsFrom(List<AssigneeDTO> assigneeDTOs, Card card)
        {
            var assigneeCards = new List<AssigneeCard>();
            foreach (var assigneeDTO in assigneeDTOs)
            {
                var assignee = _mapper.Map<Assignee>(assigneeDTO);

                var newAssigneeCard = new AssigneeCard(assignee, card);
                assigneeCards.Add(newAssigneeCard);
            }

            return assigneeCards;
        }

        private List<AssigneeProject> extractAssigneeProjectsFrom(ProjectWithCardsDTO projectDTO, Project project)
        {
            var assigneeProjects = new List<AssigneeProject>();
            foreach (var assigneeDTO in projectDTO.AssigneeDTOs)
            {
                var newAssigneeProject = new AssigneeProject(_mapper.Map<Assignee>(assigneeDTO), project);
                assigneeProjects.Add(newAssigneeProject);
            }
            return assigneeProjects;
        }

        public void Delete(Guid projectId)
        {
            _myContext.AssigneeProjects.RemoveRange(_myContext.AssigneeProjects.Where(ap => ap.ProjectId.Equals(projectId)));
            Guid? projectIdNullable = projectId;
            _myContext.Cards.RemoveRange(_myContext.Cards.Where(c => c.Project.Id.Equals(projectIdNullable)));
            _myContext.Projects.Remove(_myContext.Projects.First(p => p.Id.Equals(projectId)));
            _myContext.SaveChanges();
        }
    }
}