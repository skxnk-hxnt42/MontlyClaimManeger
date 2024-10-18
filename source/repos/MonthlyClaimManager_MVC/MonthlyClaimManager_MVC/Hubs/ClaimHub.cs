using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using MonthlyClaimManager.Hubs;


namespace MonthlyClaimManager.Hubs
{
    public class ClaimHub : Hub
    {
        public async Task NotifyNewClaim()
        {
            await Clients.All.SendAsync("ReceiveClaimUpdate");
        }

        public async Task NotifyClaimStatusChange()
        {
            await Clients.All.SendAsync("ReceiveStatusUpdate");
        }
    }
}
