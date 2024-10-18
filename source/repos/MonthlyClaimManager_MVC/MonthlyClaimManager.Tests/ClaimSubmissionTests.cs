using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MonthlyClaimManager.Controllers;
using MonthlyClaimManager.Models;
using Microsoft.AspNetCore.SignalR;
using MonthlyClaimManager.Hubs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MonthlyClaimManager.Tests
{
    public class ClaimControllerTests
    {
        private readonly Mock<IHubContext<ClaimHub>> _mockHubContext;
        private readonly ClaimController _controller;

        public ClaimControllerTests()
        {
            _mockHubContext = new Mock<IHubContext<ClaimHub>>();
            _controller = new ClaimController(_mockHubContext.Object, Mock.Of<ILogger<ClaimController>>());
        }

        [Fact]
        public async Task SubmitClaim_ValidClaim_ReturnsRedirectToClaimsList()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerID = 1,
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 50
            };

            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Test file content");
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.ContentType).Returns("application/pdf");

            // Act
            var result = await _controller.SubmitClaim(claim, fileMock.Object);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ClaimsList", redirectResult.ActionName);
        }
    }
}
