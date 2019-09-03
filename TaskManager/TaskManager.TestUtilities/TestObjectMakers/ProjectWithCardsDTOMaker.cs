using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TaskManager.Models.DTOs;

namespace TaskManager.TestUtilities.TestObjectMakers
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

