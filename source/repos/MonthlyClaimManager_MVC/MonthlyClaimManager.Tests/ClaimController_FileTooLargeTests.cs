using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MonthlyClaimManager.Controllers;
using MonthlyClaimManager.Models;
using Microsoft.AspNetCore.SignalR;
using MonthlyClaimManager.Hubs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonthlyClaimManager.Tests
{
    public class ClaimController_NoFileTests
    {
        private readonly Mock<IHubContext<ClaimHub>> _mockHubContext;
        private readonly ClaimController _controller;

        public ClaimController_NoFileTests()
        {
            _mockHubContext = new Mock<IHubContext<ClaimHub>>();
            _controller = new ClaimController(_mockHubContext.Object, Mock.Of<ILogger<ClaimController>>());
        }

        [Fact]
        public async Task SubmitClaim_NoFileUploaded_ReturnsErrorMessage()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerID = 1,
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 50
            };

            // Act
            var result = await _controller.SubmitClaim(claim, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(_controller.ModelState.ContainsKey(string.Empty));
            Assert.Equal("Please upload a supporting document.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }
    }
}
