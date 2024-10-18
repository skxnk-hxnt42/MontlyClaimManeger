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
    public class ClaimController_ApproveTests
    {
        private readonly Mock<IHubContext<ClaimHub>> _mockHubContext;
        private readonly ClaimController _controller;

        public ClaimController_ApproveTests()
        {
            _mockHubContext = new Mock<IHubContext<ClaimHub>>();
            _controller = new ClaimController(_mockHubContext.Object, Mock.Of<ILogger<ClaimController>>());
        }

        [Fact]
        public async Task ApproveClaim_ChangesStatusToApproved()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerID = 1,
                LecturerName = "John Doe",
                ClaimStatus = "Pending",
                HoursWorked = 5,
                HourlyRate = 100
            };

            // Act: Submit the claim first
            var submitResult = await _controller.SubmitClaim(claim, null);

            // Act: Approve the claim
            var approveResult = await _controller.ApproveClaim(claim.LecturerID);

            // Assert: Ensure it redirects to ClaimsList after approval
            var redirectResult = Assert.IsType<RedirectToActionResult>(approveResult);
            Assert.Equal("ClaimsList", redirectResult.ActionName);

            // Check if the claim's status has been updated to "Approved"
            Assert.Equal("Approved", claim.ClaimStatus);
        }
    }
}
