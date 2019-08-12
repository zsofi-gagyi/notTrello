using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class AuthenthicationAndAuthorizationTests //: IClassFixture<TestContext>
    {
        private readonly TestContext _testContext;
        public readonly string _correctTokenForAdmin;
        private readonly string _correctTokenForUser;
        private readonly string _incorrectToken;
        private readonly HttpContent _request;

        public AuthenthicationAndAuthorizationTests(TestContext testContext)
        {
            _testContext = testContext;

            var tokenService = new TokenService();
            _correctTokenForAdmin = tokenService.GenerateToken("testAdmin", "TodoAdmin", false);  // TODO this must be researched and changed to "true"
            _correctTokenForUser = tokenService.GenerateToken("testUser", "TodoUser", false);
            _incorrectToken = tokenService.GenerateToken("testUser", "IncorrectRole", false);

            _request = new StringContent("testRequest");
        }

        [Theory]
        [InlineData("/api/users", "POST" )]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_TokenForUser_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _correctTokenForUser);

            var response = new HttpResponseMessage();

            switch (urlAndAction[1])
            {
                case "POST":
                    response = await _testContext.Client.PostAsync(urlAndAction[0], _request);
                    break;
                default:
                    response = await _testContext.Client.GetAsync(urlAndAction[0]);
                    break;
            }            

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/users")]
        public async Task Post_IncorrectToken_Forbidden(string url)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _incorrectToken);

            var response = await _testContext.Client.PostAsync(url, _request);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/users")]
        public async Task Post_MissingToken_Unauthorized(string url)
        {
            var response = await _testContext.Client.PostAsync(url, _request);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
