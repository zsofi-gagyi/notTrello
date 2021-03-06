﻿using AutoMapper;
using Newtonsoft.Json;
using TaskManager.TestUtilities.TestObjectMakers;
using TaskManager.Models.DTOs;
using TaskManager.Services.Extensions;
using Xunit;

namespace TodoWithDatabase.UnitTests.Services
{
    public class MappingTests  
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _mapper = AutoMapperSetup.CreateMapper();
        }

        [Fact]
        public void Assignee_To_AssigneeWithProjectsDTO_Translates()
        {
            var assignee = AssigneeMaker.MakeAssigneeWithProject();

            var expectedDTO = AssigneeWithProjectsDTOMaker.MakeFrom(assignee);
            var expectedString = JsonConvert.SerializeObject(expectedDTO);

            var resultingDTO = _mapper.Map<AssigneeWithProjectsDTO>(assignee);
            var resultString = JsonConvert.SerializeObject(resultingDTO);

            Assert.Equal(expectedString, resultString);
        }
    }
}
