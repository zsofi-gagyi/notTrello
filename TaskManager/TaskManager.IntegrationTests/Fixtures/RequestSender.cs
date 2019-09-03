using System.Net.Http;
using System.Threading.Tasks;

namespace TaskManager.IntegrationTests.Fixtures
{
    public static class RequestSender
    {
        public static async Task<HttpResponseMessage> Send(HttpClient client, HttpContent request, string[] urlAndAction)
        {
            var response = new HttpResponseMessage();
            switch (urlAndAction[1])
            {
                case "GET":
                    response = await client.GetAsync(urlAndAction[0]);
                    break;
                case "POST":
                    response = await client.PostAsync(urlAndAction[0], request);
                    break;
                case "PUT":
                    response = await client.PutAsync(urlAndAction[0], request);
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(urlAndAction[0]);
                    break;
            }
            return response;
        }
    }
}
