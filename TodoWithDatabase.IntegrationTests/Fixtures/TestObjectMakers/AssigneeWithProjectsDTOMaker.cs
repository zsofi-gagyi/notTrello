using System;
using System.Collections.Generic;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.UnitTests.Fixtures.TestObjectMakers
{
    public static class AssigneeWithProjectsDTOMaker
    {
        public static AssigneeWithProjectsDTO Make()
        {
            var assignee1 = new AssigneeDTO { Id = "1", UserName =  "user1Name" };
            var assignee2 = new AssigneeDTO { Id = "2", UserName = "user2Name" };

            var card3 = new CardWithAssigneesDTO
            {
                Id = "00000000-0000-0000-0000-000000000003",
                Title = "card3Title",
                Description = "card3Description",
                Deadline = DateTime.Parse("2019-12-08T12:00:00"),
                Done = false
            };
            card3.AssigneeDTOs = new List<AssigneeDTO> { assignee1 };

            var project4 = new ProjectWithCardsDTO
            {
                Id = "00000000-0000-0000-0000-000000000004",
                Title = "project4Title",
                Description = "project4Description",
                CardWithAssigneeDTOs = new List<CardWithAssigneesDTO> { card3 }
            };

            project4.AssigneeDTOs = new List<AssigneeDTO> { assignee1, assignee2 };

            var assigneeWithProjects = new AssigneeWithProjectsDTO
            {
                Id = "1",
                UserName = "user1Name",
                Role = null,                                                      // this will is overwritten in the actual AssigneeService method
                ProjectWithCardsDTOs = new List<ProjectWithCardsDTO> { project4 }
            };

            return assigneeWithProjects;
        }
    }
}
