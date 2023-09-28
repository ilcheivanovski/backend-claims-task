using Claims.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        [Fact]
        public async Task Get_Claims()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                { });

            var client = application.CreateClient();

            var response = await client.GetAsync("/Claims");

            response.EnsureSuccessStatusCode();

            //TODO: Apart from ensuring 200 OK being returned, what else can be asserted?

            var contentString = await response.Content.ReadAsStringAsync();
            var claimsResponse = JsonConvert.DeserializeObject<Claims.Services.Claims.GetAllClaims.Response>(contentString);

            Assert.NotNull(claimsResponse.Claims);
            Assert.True(claimsResponse.Claims.Any());
        }
        [Fact]
        public async Task Create_Claim_Success()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                { });

            var client = application.CreateClient();

            Claims.Services.Claims.CreateClaim.Request claim = new Services.Claims.CreateClaim.Request()
            {
                CoverId = "1",
                DamageCost = 4,
                Name = "1",
                Type = ClaimType.Grounding
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Claims", jsonContent);

            response.EnsureSuccessStatusCode();


            var contentString = await response.Content.ReadAsStringAsync();
            var claimsResponse = JsonConvert.DeserializeObject<Claims.Services.Claims.CreateClaim.Response>(contentString);

            Assert.NotNull(claimsResponse.Id);
            Assert.True(claimsResponse != null);
        }
        [Fact]
        public async Task Create_Claim_Bad_Request_No_CoverId()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                { });

            var client = application.CreateClient();

            Claims.Services.Claims.CreateClaim.Request claim = new Services.Claims.CreateClaim.Request()
            {
                DamageCost = 4,
                Name = "1",
                Type = ClaimType.Grounding
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Claims", jsonContent);


            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
