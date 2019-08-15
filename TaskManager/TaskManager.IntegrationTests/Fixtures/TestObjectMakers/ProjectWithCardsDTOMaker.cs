using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTOs;

namespace TaskManager.IntegrationTests.Fixtures.TestObjectMakers
{
    public static class ProjectWithCardsContentMaker
    {
        public static HttpContent MakeStringContentWith(string Id)
        {
            var contentObject = new ProjectWithCardsDTO
            {
                Id = Id,
                Title = "changed",
                CardWithAssigneesDTOs = new List<CardWithAssigneesDTO>(),
                AssigneeDTOs = new List<AssigneeDTO>()
            };

            return new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
        }
    }
}

