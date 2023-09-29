using Claims.Api.Controllers;
using Claims.Core;
using Claims.Services.Claims;
using MediatR;
using Moq;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTest
    {
        private readonly string claimId = Guid.NewGuid().ToString();

        [Fact]
        public async Task Get_Claims()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllClaims.Request>(), default))
                        .ReturnsAsync(GetTestSessions()); // Mock the query response

            var controller = new ClaimsController(mediatorMock.Object);

            // Act
            var result = await controller.GetAllClaims();

            // Assert
            Assert.True(result.Claims.Any());

        }

        [Fact]
        public async Task Create_Claim_Success()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<CreateClaim.Request>(), default))
                        .ReturnsAsync(CreateClaimResponse()); // Mock the query response

            var controller = new ClaimsController(mediatorMock.Object);

            // Act
            Claims.Services.Claims.CreateClaim.Request claim = new Services.Claims.CreateClaim.Request()
            {
                CoverId = "1",
                DamageCost = 4,
                Name = "1",
                Type = ClaimType.Grounding
            };
            var result = await controller.CreateClaim(claim);

            // Assert
            Assert.True(result.Name.Equals(claim.Name));
        }

        [Fact]
        public async Task Get_Claim_Damage_cost_less_than_100000()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetClaim.Request>(), default))
                        .ReturnsAsync(GetClaimResponse()); // Mock the query response

            var controller = new ClaimsController(mediatorMock.Object);

            // Act
            var result = await controller.GetClaim(claimId);

            // Assert
            Assert.True(result.DamageCost < 100000);
        }

        private GetAllClaims.Response GetTestSessions()
        {
            var claims = new List<GetAllClaims.Response.ClaimItem>();
            claims.Add(new GetAllClaims.Response.ClaimItem()
            {
                Id = Guid.NewGuid().ToString(),
                CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                DamageCost = 1,
                Name = "Test",
                Type = ClaimType.BadWeather
            });
            claims.Add(new GetAllClaims.Response.ClaimItem()
            {
                Id = Guid.NewGuid().ToString(),
                CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                DamageCost = 5,
                Name = "Test2",
                Type = ClaimType.Grounding
            });
            return new GetAllClaims.Response() { Claims = claims };
        }
        private CreateClaim.Response CreateClaimResponse()
        {
            Claims.Services.Claims.CreateClaim.Response claim = new Services.Claims.CreateClaim.Response()
            {
                Id = Guid.NewGuid().ToString(),
                CoverId = "1",
                DamageCost = 4,
                Name = "1",
                Type = ClaimType.Grounding,
                Created = DateTime.Now,
            };
            return claim;
        }
        private GetClaim.Response GetClaimResponse()
        {
            Claims.Services.Claims.GetClaim.Response claim = new Services.Claims.GetClaim.Response()
            {
                Id = claimId,
                CoverId = "1",
                DamageCost = 4,
                Name = "1",
                Type = ClaimType.Grounding,
                Created = DateTime.Now,
            };
            return claim;
        }
    }
}
